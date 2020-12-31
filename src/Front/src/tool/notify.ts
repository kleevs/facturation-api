import * as toastr from 'toastr';

export function error(title: string, message: string) {
    toastr.error(message, title);
}

export function success(title: string)
export function success(title: string, message: string)
export function success(title: string, message?: string) {
    toastr.success(message || '', title);
}

toastr.options = {
    "closeButton": false,
    "debug": false,
    "newestOnTop": false,
    "progressBar": false,
    "positionClass": "toast-bottom-right",
    "preventDuplicates": false,
    "onclick": null,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
  }