function foreach(item, callback) {
    let i;
    if (item instanceof Array) {
        for (i = 0; i < item.length; i++) {
            callback(item[i], i);
        }
    }
    else {
        for (i in item) {
            callback(item[i], i);
        }
    }
}

function contains(array, item) {
    let res = false;
    foreach(array, (x) => { res = res || item === x; });
    return res;
}

let stack: { func: Function}[] = [];

function push(func: Function) {
	stack.push({ func: func });
}

function pop(): { func: Function } {
	return stack.pop();
}

function peek(): { func: Function } {
	return stack[stack.length-1];
}

export function create<T>(fn: ()=>T): () => T {
    var listeners = [],
		defaultValue = {},
		value: T = <any>defaultValue;

	return function () {
		var observer = peek() && peek().func, 
			firstCall = defaultValue === value;

		if (observer && !contains(listeners, observer)) {
			listeners.push(observer);
		}

		if (observer && !firstCall) {
			return value;
		}

		if (value !== (value=fn.apply(this, arguments)) && !firstCall) {
			var tmp = listeners;
			listeners = [];
			tmp.forEach((observer) => observer());
		}

		return value;
	};
}

export function observer<T>(fn: ()=>T): void {
	var me;
	(me = () => {
		push(me);
		var res = fn();
		pop();
		return res;
	})();
}

export function startObserver<T>(fn: ()=>T): ()=>void {
	var me;
	me = () => {
        push(me);
        var res = fn();
		pop();
    };
    
    push(me);
    return () => {
        pop();
    }
}

export function blind<T>(fn: ()=>T): void {
	var me;
	(me = () => {
		push(null);
		var res = fn();
		pop();
		return res;
	})();
}

export function observable<T>(): (value?: T) => T;
export function observable<T>(value: T): (value?: T) => T; 		
export function observable<T>(value?: T): (value?: T) => T {
	var result = value;
	var obj = create(() => result);
	return function (value?: T) {
		arguments.length > 0 && (result = value);
		obj.apply(this);
		return result;
	};
}
