import { useState, useEffect } from 'react';
import RenderDataComponent from './RenderDataComponent';
import Navbar from '../Navbar/Navbar';


interface BankIdAuth {
  orderRef: string;
  autoStartToken: string;
  qrStartToken: string;
  qrStartSecret: string;
}

const AuthCall = () => {
  const [data, setData] = useState<BankIdAuth>({ orderRef: '', autoStartToken: '', qrStartToken: '', qrStartSecret: '' } as BankIdAuth);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const request = {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({ endUserIp: '192.168.0.1' }),
        };
        const response = await fetch('https://localhost:7080/api/BankAuth', request);

        if (!response.ok) {
          console.error('API request failed:', response.status, response.statusText);

          // Handle response content for debugging purposes
          const responseContent = await response.text();
          console.error('Response content:', responseContent);

          //throw new Error('API request failed');
        }


        const responseData = await response.json();
        setData(responseData);
      } catch (error) {
        console.error('Error fetching data:', error);
      }
    };

    fetchData();
  }, []);

  return (
    <>
      <Navbar />
      {data ? <RenderDataComponent data={data} orderTime={new Date()} /> : <p>Loading...</p>}
    </>
  )
};




export default AuthCall;