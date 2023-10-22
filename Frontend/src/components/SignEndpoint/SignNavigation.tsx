import Navbar from "../Navbar/Navbar";
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import Button from 'react-bootstrap/Button';
import { useNavigate } from 'react-router-dom';

import { useEffect, useState } from "react";

const SignNavigation = () => {

    const [authData, setAuthData] = useState<boolean>(false);
    
    const navigate = useNavigate();

    const authorize = () => {
        setAuthData(true);
    }

    useEffect(() => {
        if (authData === true) {
            navigate('/sign')
        }
    }, [authData])

   




    return (
        <div>
            <Navbar />
            <Container>
                <Row>
                    <Col>
                        <Button variant="primary" size="lg" active onClick={authorize}>Signera</Button>
                    </Col>
                </Row>
            </Container>
        </div>
    )
}

export default SignNavigation;