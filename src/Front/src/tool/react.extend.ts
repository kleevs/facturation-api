import { useState, useEffect } from 'react';

var getOwnPropertyDescriptor = (obj, key: string) => {
    if (!obj) {
        return null;
    }

    var desc = Object.getOwnPropertyDescriptor(obj, key);
    return desc || getOwnPropertyDescriptor(Object.getPrototypeOf(obj), key);
}

var currentObserver;
function observalizer<T>(obj: T) {
    if (obj) {
        for (var key in obj) {
            ((obj, key) => {
                var desc = getOwnPropertyDescriptor(obj, key);
                if (key === "nouveauDateCreation") {
                    console.log("ok");
                }
                if (!desc || !(desc.get || desc.set) && desc.configurable) {
                    var value = obj[key];
                    var listeners = [];
                    (value instanceof Object) && observalizer(value);
                    Object.defineProperty(obj, key, {
                        get: () => { 
                            currentObserver && (listeners.indexOf(currentObserver)<0) && listeners.push(currentObserver); 
                            return value; 
                        },
                        set: (v) => { 
                            value = v;
                            (v instanceof Object) && observalizer(v);
                            listeners.forEach(_ => _());
                        }
                    });
                }
            })(obj, key);
        }
    }
}

var observers = {};
export function useObserver<T>(obj: T) : T {
    const [, setValue] = useState(0);
    const [id] = useState(`id${new Date().getTime()}${Math.random()*100}`);

    var observer = observers[id] = observers[id] || (() => {
        setValue({ value: obj });
    });

    const previousObserver = currentObserver;
    currentObserver = observer;
    observalizer(obj);
    useEffect(() => {
        currentObserver = previousObserver;
    });

    return obj;
}

var inputFileValueDescriptor = Object.getOwnPropertyDescriptor(HTMLInputElement.prototype, "value");

Object.defineProperty(HTMLInputElement.prototype, "value", {
    configurable: true,
    get: function () {
        return inputFileValueDescriptor.get.call(this);
    },
    set: function (value) {
        if (this.type == "file") {
            if (!value || value.length <= 0) {
                inputFileValueDescriptor.set.call(this, '');
            }
        } else {
            inputFileValueDescriptor.set.call(this, value);
        }
    }
});

// export function Router(props: {children?; history}) {
//     // var [data, setLocation] = useState({ authData: null, location: window.location });

//     // useEffect(() => { 
//     //     var unlisten = props.history.listen((location) => {
//     //         setLocation({ authData: null, location });
//     //     });
//     //     return () => {
//     //         unlisten();
//     //     }
//     // });
//     // !data.authData && props.history.isConnected().then((authData) => { 
//     //     if (authData) {
//     //         setLocation({ authData, location: data.location }); 
//     //     }
//     // });
//     return props.children;
// }

// export function Route(props: {exact?:boolean; anonymous?:boolean; path:string; render: (props)=>any}) {
//     var pathname = location.pathname;
//     var regex = new RegExp(props.path.replace(/:(\w+)/, '(?<$1>[^/]+)'));
//     var res = regex.exec(pathname);
//     return res && props.render({ match: { params: res.groups }}) || '';
// }

// export function RouteAsync(props: {exact?:boolean; path:string; render: (props)=>Promise<any>}) {
//     var [dom, setDom] = useState(null);

//     return Route({
//         exact: props.exact,
//         path: props.path,
//         render: (p) => { 
//             props.render(p).then(_ => setDom(_)).catch(() => {});
//             return '';
//         }
//     });
// }
