
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Home from './Home';

import 'bootstrap/dist/css/bootstrap.min.css';
import AuthCall from './components/AuthEndpoint/AuthCall';
import AuthNavigation from './components/AuthEndpoint/AuthNavigation';
import SignNavigation from './components/SignEndpoint/SignNavigation';
import SignCall from './components/SignEndpoint/SignCall';
function App() {


  return (
    <BrowserRouter >
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/auth" element={<AuthCall />} />
        <Route path="/sign" element={<SignCall />} />
        <Route path="/authcall" element={<AuthNavigation />} />
        <Route path="/signcall" element={<SignNavigation />} />
      </Routes>
    </BrowserRouter>
  )
}

export default App
