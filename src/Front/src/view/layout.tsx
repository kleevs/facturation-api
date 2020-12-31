import * as React from 'react';
import { Layout } from '../app/page/layout';
 
export default function(options: { app: Layout})
export default function(options: { children?, app: Layout }) {
    var app = options.app;
    var children = options.children;
    var link = {
        home: '/',
        nouvelleFacture: '/facturations/new',
        factures: '/facturations',
        account: '/account'
    };
    return <div id="wrapper">

      <ul className="navbar-nav bg-gradient-primary sidebar sidebar-dark accordion" id="accordionSidebar">

        <a className="sidebar-brand d-flex align-items-center justify-content-center" href={link.home}>
          <div className="sidebar-brand-icon rotate-n-15">
            <i className="fas fa-laugh-wink"></i>
          </div>
          <div className="sidebar-brand-text mx-3">MFacture</div>
        </a>

        <hr className="sidebar-divider my-0" />

        {/* <li className="nav-item active">
          <a className="nav-link" href="/">
            <i className="fas fa-fw fa-tachometer-alt"></i>
            <span>Dashboard</span></a>
        </li>

        <hr className="sidebar-divider" /> */}

        {/* <div className="sidebar-heading">
          Facturation
        </div> */}

        <li className="nav-item">
          <a className="nav-link" href={link.nouvelleFacture} aria-expanded="true">
            <i className="fa fa-plus-circle"></i>
            <span>Nouvelle facture</span>
          </a>
        </li>

        <li className="nav-item">
          <a className="nav-link" href={link.factures} aria-expanded="true">
            <i className="fa fa-list-alt"></i>
            <span>Factures enregistrées</span>
          </a>
        </li>

        <hr className="sidebar-divider" />
      </ul>

      <div id="content-wrapper" className="d-flex flex-column">

        <div id="content">

          <nav className="navbar navbar-expand navbar-light bg-white topbar mb-4 static-top shadow">

            <button id="sidebarToggleTop" className="btn btn-link d-md-none rounded-circle mr-3">
              <i className="fa fa-bars"></i>
            </button>

            <ul className="navbar-nav ml-auto">
              <div className="topbar-divider d-none d-sm-block"></div>

              <li className="nav-item dropdown no-arrow">
                <a className="nav-link dropdown-toggle" href="#" id="userDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                  <span className="mr-2 d-none d-lg-inline text-primary small">{app && app.login || ''}</span>
                </a>
                
                <div className="dropdown-menu dropdown-menu-right shadow animated--grow-in" aria-labelledby="userDropdown">
                  <a className="dropdown-item text-primary" href={link.account}>
                    <i className="fa fa-user"></i>&nbsp;
                    Mon compte
                  </a>
                  <div className="dropdown-divider"></div>
                  <a className="dropdown-item text-primary" href="#" onClick={() => app.logout()} data-toggle="modal" data-target="#logoutModal">
                    <i className="fas fa-sign-out-alt fa-sm fa-fw mr-2 text-gray-400"></i>
                    Se déconnecter
                  </a>
                </div>
              </li>
            </ul>
  
          </nav>

          {children}
        </div>

        <footer className="sticky-footer bg-white">
          <div className="container my-auto">
            <div className="copyright text-center my-auto">
              <span>Copyright &copy; mfacture.fr 2019</span>
            </div>
          </div>
        </footer>
      </div>
    </div>;
}