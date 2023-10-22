import React, { useEffect, useState } from 'react';
import * as CryptoJS from 'crypto-js';
import QRCode from 'qrcode.react';
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import Button from 'react-bootstrap/Button';
import InMemoryJwtManager from './CallAuth/InMemoryJwtManager';
import { UserMessages } from '../UserMessages/UserMessages';
import { isIOS, isAndroid, isChrome, isSafari } from 'react-device-detect';

interface RenderDataProps {
    data: BankIdAuth;
    orderTime: Date;
}

interface BankIdAuth {
    orderRef: string;
    autoStartToken: string;
    qrStartToken: string;
    qrStartSecret: string;
}

interface ApiResponse {
    orderRef: string;
    status: 'pending' | 'failed' | 'complete';
    hintCode?: string;
    token?: string; // Adding the token property for success
    errorCode?: string;
}

const RenderDataComponent: React.FC<RenderDataProps> = ({ data, orderTime }) => {
    const [qrData, setQrData] = useState('');
    const [, setQrTime] = useState(0);
    const [apiContent, setApiContent] = useState<ApiResponse | null>(null);
    const [hintCode, setHintCode] = useState<string | null>(null);
    const [userMessage, setUserMessage] = useState<string | null>(null);
    const [canceledRequest, setCanceledRequest] = useState<boolean>(false);







    const initiateAuthentication = () => {
        fetch('https://localhost:7080/api/Collect', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ orderRef: data.orderRef }),
        })
            .then(response => response.json())
            .then(apiData => {
                const userMessage = UserMessages(apiData);
                setUserMessage(userMessage);
                setHintCode(apiData.hintCode ? apiData.hintCode : null);
                setApiContent(apiData);
                InMemoryJwtManager.setToken(apiData.token);

                // Retrieve the return URL from the URL parameters
                const returnUrl = new URLSearchParams(window.location.search).get('returnUrl');

                // Redirect the user back to the return URL
                if (apiData.status === 'complete' && returnUrl) {
                    //window.location.href = returnUrl;
                }
            })
            .catch(error => {
                console.error('API request failed:', error);
            });
    };


    const getQrAuthCode = (qrStartSecret: string, time: number): string => {
        const keyByteArray = CryptoJS.enc.Utf8.parse(qrStartSecret);
        const hmac = CryptoJS.HmacSHA256(time.toString(), keyByteArray);
        return hmac.toString(CryptoJS.enc.Hex);
    };

    const cancelRequest = (): void => {
        fetch('https://localhost:7080/api/Cancel', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ orderRef: data.orderRef }),
        })
            .then(response => response.json())
            .then(canceled => {
                console.log(canceled);
                if (Object.keys(canceled).length === 0) {
                    setCanceledRequest(true);
                }
            })
    }

    useEffect(() => {
        if (canceledRequest === false) {

            const updateQRData = () => {
                const orderTimes = orderTime;
                const newQrTime = Math.floor((new Date().getTime() - orderTimes.getTime()) / 1000);
                setQrTime(newQrTime);

                const qrAuthCode = getQrAuthCode(data.qrStartSecret, newQrTime);

                const qrData = `bankid.${data.qrStartToken}.${newQrTime}.${qrAuthCode}`;
                setQrData(qrData);
            };

            updateQRData();
        }
    }, [data, orderTime]);

    useEffect(() => {

        if (canceledRequest === false) {
            const interval = setInterval(() => {
                if (apiContent && (apiContent.status === 'failed' || apiContent.status === 'complete')) {
                    console.log(apiContent);
                    clearInterval(interval);
                    return;
                }
                else {
                    console.log(canceledRequest)


                    fetch('https://localhost:7080/api/Collect', {
                        method: 'POST',
                        headers: { 'Content-Type': 'application/json' },
                        body: JSON.stringify({ orderRef: data.orderRef }),
                    })
                        .then(response => response.json())
                        .then(apiData => {
                            // console.log(apiData);
                            const userMessage = UserMessages(apiData);
                            setUserMessage(userMessage);
                            setHintCode(apiData.hintCode ? apiData.hintCode : null);
                            setApiContent(apiData);
                            InMemoryJwtManager.setToken(apiData.token);
                        })
                        .catch(error => {
                            console.error('API request failed:', error);
                        });
                }
            }, 2000);
            return () => clearInterval(interval);
        }
    }, [data, apiContent]);

    const startFromFile = () => {

        //const returnUrl = encodeURIComponent(window.location.href);
        //TODO: fix the returnUrl
        const returnUrl = '';
        if (isIOS || isAndroid) {

            if (isChrome || isSafari) {
                const url = `https://app.bankid.com/?autostarttoken=${data.autoStartToken}&redirect=${returnUrl}`;
                window.location.href = url;
            }
        }
        else {
            console.log('not mobile')
            const url = `bankid:///?autostarttoken=${data.autoStartToken}&redirect=${returnUrl}`;
            console.log(url);
            console.log(returnUrl)
            //window.location.href = url;
        }

        initiateAuthentication();
    };

    return (
        <Container>
            <Row>
                <Col>
                    <p>Generated QR Data:</p>
                    <QRCode value={qrData} />
                </Col>
            </Row>
            <Row>
                <Col>
                    <Button variant="primary" onClick={startFromFile}>
                        Login from file
                    </Button>
                </Col>
            </Row>
            <Row>
                <Col className='mt-3'>
                    <Button variant="secondary" onClick={cancelRequest} >Cancel Request</Button>
                </Col>
            </Row>
            {apiContent && !canceledRequest && (
                <Row>
                    <Col>
                        {apiContent.status === 'complete' && apiContent.token ? (
                            <div>
                                <p>API Status: Success! Token: {apiContent.token}</p>
                                <p>Token: {InMemoryJwtManager.getToken()}</p>
                            </div>

                        ) : (
                            <p>API Status: {apiContent.status}</p>
                        )}
                        {hintCode &&
                            <p>Hint code: {hintCode}</p>
                        }
                        {userMessage &&
                            <p>User message: {userMessage}</p>
                        }

                    </Col>
                </Row>
            )}
            {canceledRequest === true &&
                <div>Request was canceled</div>
            }
        </Container>
    );
};

export default RenderDataComponent;