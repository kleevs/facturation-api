import * as React from 'react';
import { Login as App } from '../app/page/login';
import { useObserver } from '../tool/react.extend';
import { preventDefault } from '../tool/mixin';

export default function (props: { app: App }) {
    var login = useObserver(props.app);

return <div className="container">
<div className="row justify-content-center">

  <div className="col-xl-10 col-lg-12 col-md-9">

    <div className="card o-hidden border-0 shadow-lg my-5">
      <div className="card-body p-0">
        <div className="row">
          <div className="col-lg-6 d-none d-lg-block bg-login-image"></div>
          <div className="col-lg-6">
            <div className="p-5">
              <div className="text-center">
                <h1 className="h4 text-gray-900 mb-4">Welcome Back!</h1>
              </div>
              <form className="user" onSubmit={(e) => preventDefault(e, () => login.signin())}>
                <div className="form-group">
                  <input type="email" value={login.email} onChange={(e) => login.email = e.target.value} className="form-control form-control-user" id="exampleInputEmail" aria-describedby="emailHelp" placeholder="Enter Email Address..."/>
                </div>
                <div className="form-group">
                  <input type="password" value={login.password} onChange={(e) => login.password = e.target.value} className="form-control form-control-user" id="exampleInputPassword" placeholder="Password"/>
                </div>
                <div className="form-group">
                  <div className="custom-control custom-checkbox small">
                  </div>
                </div>
                <button type="submit" href="index.html" className="btn btn-primary btn-user btn-block">
                  Login
                </button>
              </form>
            </div>
          </div>
        </div>
      </div>
    </div>

  </div>

</div>

</div>
}