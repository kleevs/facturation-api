import * as $ from 'jquery';
import { parseDate } from './date';
import { apiDomain as url } from '../config';
import { error } from './notify';

function JSONParse(arg) {
    return JSON.parse(arg, (key, value) => parseDate(value));
}

$( document ).ajaxError(( event, request, settings ) => {
    if (request.responseJSON && request.responseJSON && request.responseJSON.message) {
        error("Erreur", request.responseJSON.message);
    }
  });

function _ajax<T>(url: string, options: {
    method: string;
    headers?: {[s: string]: string},
    data?: any;
    cache?: boolean;
}, conf: {
    contentType?: any;
    processData?: boolean;
}) {
    return new Promise<{ result: T, status: string }>((resolve, reject) => {
        $.ajax({
            method: options.method,
            url: `${apiDomain}${url}`,
            data: options.data,
            dataType: 'json',
            contentType: conf.contentType,
            processData: conf.processData,
            xhrFields: {
                withCredentials: true
            },
            cache: options.cache || false,
            converters: {
                "text json": JSONParse
            },
            crossDomain: true,
            success: (data, textStatus, xhr) => {
                resolve({ result: data, status: xhr.status });
            }, 
            error: (xhr, textStatus) => {
                var typeStatus = parseInt(`${xhr.status / 100}`) || 5;
                if (typeStatus === 2) {
                    resolve({ result: undefined, status: xhr.status });
                } else {
                    if (typeStatus === 4) {
                    } else if (typeStatus === 5) {
                        error(textStatus, "Une erreur est survenue.");
                    }
                    var json = undefined;
                    try {
                        json = JSON.parse(xhr.responseText);
                    } catch (e) {}
                    
                    reject({ result: json || xhr.responseText, status: xhr.status });
                }
            }, 
          });
    });
}

export let apiDomain = url;
export function ajax<T>(url: string, options: {
    method: string;
    headers?: {[s: string]: string},
    data?: any;
    cache?: boolean;
}) {
    return  _ajax<T>(url, {
        method: options.method,
        headers: options.headers,
        data: JSON.stringify(options.data),
        cache: options.cache
    }, {
        contentType: 'application/json'
    });
}

export function ajaxFormData<T>(url: string, options: {
    method: string;
    headers?: {[s: string]: string},
    data?: {[key: string]: any;};
    cache?: boolean;
}) {
    var data = new FormData();
    if (options && options.data) {
        for (var key in options.data) {
            if (options.data[key] instanceof Array) {
                options.data[key].forEach(_ => {
                    data.append(key, _); 
                });
            } else {
                data.append(key, options.data[key]); 
            }
        }
    }

    return  _ajax<T>(url, {
        method: options.method,
        headers: options.headers,
        data: data,
        cache: options.cache
    }, {
        contentType: false,
        processData: false
    });
}
