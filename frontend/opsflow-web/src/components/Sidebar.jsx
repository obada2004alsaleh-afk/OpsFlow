import { Link } from "react-router";

function Sidebar() {
  return (
    <nav>
      <h1>Sidebar</h1>
        <Link to="/dashboard">DashBoard</Link>
        <Link to="/sites">Sites</Link>
        <Link to="/customers">Customers</Link>
        <Link to="/assets">Assets</Link>
        <Link to="/tickets">Tickets</Link>

    </nav>
  )
}

export default Sidebar