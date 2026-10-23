import { BrowserRouter, Routes, Route  } from "react-router";
import Dashboard from "../pages/Dashboard";
import Login from "../pages/Login";
import NotFound from "../pages/NotFound";
import Sites from "../pages/Sites";
import Assets from "../pages/Assets";
import Customers from "../pages/Customers";
import Tickets from "../pages/Tickets";
import MainLayout from "../layouts/MainLayout";
import { Navigate } from "react-router";

function AppRoute()
{


 return (
       <BrowserRouter>


            <Routes>
               <Route element={<MainLayout />}>

                <Route path="/dashboard" element={<Dashboard />} />
                <Route path="/sites" element={<Sites />} />
                <Route path="/customers" element={<Customers />} />
                <Route path="/assets" element={<Assets />} />
                <Route path="/tickets" element={<Tickets />} />
                <Route path="/" element={<Navigate to="/dashboard" replace />} />
               </Route>

                <Route path="/login" element={<Login />} />
                <Route path="*" element={<NotFound />} />

            </Routes>
       </BrowserRouter>

 );




}

export default AppRoute;