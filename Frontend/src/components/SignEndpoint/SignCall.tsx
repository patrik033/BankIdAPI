import { useState, useEffect } from 'react';
//import RenderDataComponent from './RenderDataComponent';
import Navbar from '../Navbar/Navbar';
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';
import "./../../CustomStyles/Bootstrap.css"
import { pdfjs, Document } from 'react-pdf';
import RenderDataComponentSigner from './RenderDataComponentSigner';

pdfjs.GlobalWorkerOptions.workerSrc = `//unpkg.com/pdfjs-dist@${pdfjs.version}/legacy/build/pdf.worker.min.js`;



interface BankIdAuth {
  orderRef: string;
  autoStartToken: string;
  qrStartToken: string;
  qrStartSecret: string;
}

interface PdfMetadata {
  Author: string;
  CreationDate: string;
  Creator: string;
  EncryptFilterName: string | null;
  IsAcroFormPresent: boolean;
  IsCollectionPresent: boolean;
  IsLinearized: boolean;
  IsSignaturesPresent: boolean;
  IsXFAPresent: boolean;
  Language: string;
  ModDate: string;
  PDFFormatVersion: string;
  Producer: string;
}

// interface SignRequest {
//   endUserIp: string;
//   requirement?: object;
//   userVisibleData: string;
//   userNonVisibleData?: string;
//   userVisibleDataFormat?: string;
// }

const SignCall = () => {
  const [pdfBlob, setPdfBlob] = useState<Blob | null>(null);
  const [accept, setAccept] = useState<boolean>(false);
  const [metadata, setMetadata] = useState(null);
  const [userVisibleData, setUserVisibleData] = useState(null);
  const [data, setData] = useState<BankIdAuth>({ orderRef: '', autoStartToken: '', qrStartToken: '', qrStartSecret: '' } as BankIdAuth);

  useEffect(() => {
    fetch('https://localhost:7080/api/Sign')
      .then(response => response.blob())
      .then(blob => setPdfBlob(blob))
      .catch(error => {
        console.error('Error fetching PDF file:', error);
      });
  }, []);

  const handleDownload = () => {
    if (pdfBlob) {
      // Create a URL for the blob
      const blobURL = URL.createObjectURL(pdfBlob);
      // Open a new window with the PDF blob URL
      window.open(blobURL, '_blank');
      // Clean up the blob URL
      URL.revokeObjectURL(blobURL);
    }
  };

  const handleAccept = () => {
    if (accept === true)
      setAccept(false)
    else
      setAccept(true)
  }

  const parsePdfMetadata = (metadata: any): PdfMetadata => {
    return {
      Author: metadata.Author,
      CreationDate: metadata.CreationDate,
      Creator: metadata.Creator,
      EncryptFilterName: metadata.EncryptFilterName,
      IsAcroFormPresent: metadata.IsAcroFormPresent,
      IsCollectionPresent: metadata.IsCollectionPresent,
      IsLinearized: metadata.IsLinearized,
      IsSignaturesPresent: metadata.IsSignaturesPresent,
      IsXFAPresent: metadata.IsXFAPresent,
      Language: metadata.Language,
      ModDate: metadata.ModDate,
      PDFFormatVersion: metadata.PDFFormatVersion,
      Producer: metadata.Producer,
    };
  };


  const fetchData = async () => {


    try {
      const request = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ endUserIp: '192.168.0.1', userVisibleData: userVisibleData, userVisibleDataFormat: "simpleMarkdownV1" }),
      };
      const response = await fetch('https://localhost:7080/api/Sign', request);

      const responseData = await response.json();
      if (response.ok) {
        //console.log(responseData)
      }


      setData(responseData);

    } catch (error) {
      console.error('Error fetching data:', error);
    }

  };

  const onDocumentLoadSuccess = async (pdf: any) => {
    setMetadata(await pdf.getMetadata());
  }

  useEffect(() => {
    if (metadata) {


      const parsedMetadata = parsePdfMetadata(metadata.info);

      //const userName = "JohnnyBoy"




      const message = `# Overview
      By signing this document, you agree to the following contents of the document:
      
      ## Document metadata
      
      *Author*

      + ${parsedMetadata.Author}.

      *Creation date*

      + ${parsedMetadata.CreationDate}.

      *Language*

      + ${parsedMetadata.Language}.

      *Modification date*

      + ${parsedMetadata.ModDate}.

      ---
      Have a nice day!
      ---`;



      // const encodedTextToUtf8 = encodeURI(some)
      const encodedToBase64 = btoa(message)
      setUserVisibleData(encodedToBase64)
    }
  }, [metadata])

  useEffect(() => {
    if (userVisibleData && accept) {
      fetchData();
    }
  }, [userVisibleData, accept])








  return (
    <>
      <Navbar />
      {pdfBlob &&
        <div>
          <Document file={pdfBlob} onLoadSuccess={onDocumentLoadSuccess}>
          </Document>
        </div>
      }

      <div>
        <Container>
          <Row>
            <Col >
              <Button variant="primary" size="lg" active onClick={handleDownload}>View file</Button>
            </Col>
            <div style={{ float: "right" }}>
              Accept the contents of the file to sign it
            </div>
            <Col>
            </Col>
          </Row>
          <Row className='mt-3'>
            <Col>
              <Form.Check
                checked={accept}
                bsPrefix='form-check-input[type="checkbox"]]'
                onChange={handleAccept}
                type='checkbox'
                className='mb-3 border-dark'
                label="Check to accept the contents of the document"
              />
            </Col>
          </Row>
        </Container>
        {
          data && accept && pdfBlob &&
          <div style={{ wordBreak: "break-word" }}>
            <RenderDataComponentSigner pdf={pdfBlob} data={data} orderTime={new Date()} />
          </div>
        }
      </div>
    </>
  );
};

export default SignCall;