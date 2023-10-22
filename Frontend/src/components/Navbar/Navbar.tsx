
import Navbars from 'react-bootstrap/Navbar';

import { Container } from "react-bootstrap";
import { Link } from "react-router-dom";
import Nav from 'react-bootstrap/Nav';


const Navbar = () => {
    return (
        <Navbars expand="lg" className="bg-body-tertiary">
            <Container>
                <Navbars.Brand href="#home">React-Bootstrap</Navbars.Brand>
                <Navbars.Toggle aria-controls="basic-navbar-nav" />
                <Navbars.Collapse id="basic-navbar-nav">
                    <Nav className="me-auto">
                        <Nav.Link className="button text-dark" as={Link} to="/">Hem</Nav.Link>
                        {/* <Nav.Link className="button text-dark" as={Link} to="/auth">Auth</Nav.Link>
                        <Nav.Link className="button text-dark" as={Link} to="/sign">Sign</Nav.Link> */}
                        <Nav.Link className="button text-dark" as={Link} to="/authcall">AuthCall</Nav.Link>
                        <Nav.Link className="button text-dark" as={Link} to="/signcall">SignCall</Nav.Link>
                    </Nav>
                </Navbars.Collapse>
            </Container>
        </Navbars>
    )
}
export default Navbar;
