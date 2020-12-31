export class UI {
  constructor(private _uiController: {
    blockUI: () => void;
    unblockUI: () => void;
  }) {
  }

  blockUI() {
    return this._uiController.blockUI();
  }

  unblockUI() {
    return this._uiController.unblockUI();
  }
}