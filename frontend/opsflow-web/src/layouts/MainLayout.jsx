import { Outlet } from "react-router";
import Header from "../components/Header";
import Sidebar from "../components/Sidebar";

function MainLayout() {
  return (
    <>
      <Header />

      <div>
        <aside>
          <Sidebar />
        </aside>

        <main>
          <Outlet />
        </main>

      </div>
    </>
  );
}
export default MainLayout;