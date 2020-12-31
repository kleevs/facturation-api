import { IRouter } from '../app/spi/router';

export class RouterService implements IRouter {
    private _listener;
    location = location;
    goTo(href: string, replace?: boolean) {
        if (!replace) {
            history.pushState({}, '', href);
        } else {
            history.replaceState({}, '', href);
        }
        if (this._listener) this._listener(location);
    }

    listen(callback) {
        this._listener = callback;
    }
}