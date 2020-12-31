import { ajax, ajaxFormData } from '../../tool/ajax';
import { UI } from './ui';

export class AjaxService {
  constructor(private _uiController: UI) {}

  ajax<T>(url: string, options: {
    method: string;
    headers?: {[s: string]: string},
    data?: any;
    blockUI?: boolean;
    cache?: boolean;
  }) {
    var isBlockUI = options && options.blockUI;
    if (isBlockUI) {
        this._uiController.blockUI();
    }

    return ajax<T>(url, options)
      .finally(() => {
        if (isBlockUI) {
          this._uiController.unblockUI();
        }
      });
  }

  ajaxFormData<T>(url: string, options: {
    method: string;
    headers?: {[s: string]: string},
    data?: {[key: string]: any;};
    blockUI?: boolean;
    cache?: boolean;
  }) {
    
    var isBlockUI = options && options.blockUI;
    if (isBlockUI) {
        this._uiController.blockUI();
    }

    return ajaxFormData<T>(url, options)
      .finally(() => {
        if (isBlockUI) {
          this._uiController.unblockUI();
        }
      });
  }
}