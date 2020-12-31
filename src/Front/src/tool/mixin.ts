export function preventDefault(e: Event, callback: () => void)
export function preventDefault(e: Event, callback: () => boolean)
export function preventDefault(e: Event, callback: () => (boolean | void)) {
    var res = callback();
    !res && e.preventDefault();
}