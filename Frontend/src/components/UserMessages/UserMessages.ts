interface ApiResponse {
    orderRef: string;
    status: 'pending' | 'failed' | 'complete';
    hintCode?: string;
    token?: string; // Adding the token property for success
    errorCode?: string;
}


export const UserMessages = (response: ApiResponse) => {
    const { status, hintCode, errorCode } = response
    const message = determineMessage(status, hintCode, errorCode)
    if (message)
        return message
    else
        return "Unknown error. Please try again."
}

const determineMessage = (status?: string, hintCode?: string, errorCode?: string): string => {

    if (status === "pending" && (hintCode === "outstandingTransaction" || hintCode === "noClient"))
        return "Start your BankIID app."

    else if (errorCode === "cancelled")
        return "The BankID app is not installed. Please contact your bank."

    else if (errorCode === "cancelled")
        return "Action cancelled. Please try again."

    else if (errorCode === "alreadyInProgress")
        return "An identification or signing for this personal number is already started. Please try again."

    else if (errorCode === "requestTimeout" || errorCode === "maintenance" || errorCode === "internalError")
        return "Internal error. Please try again."

    else if (status === "failed" && hintCode === "userCancel")
        return "Action cancelled."

    else if (status === "failed" && hintCode === "expiredTransaction")
        return `The BankID app is not responding.\n
     Please check that it’s started and that you have internet access.\n
      If you don’t have a valid BankID you can get one from your bank.\n
       Try again.`

    else if (status === "pending" && hintCode === "started")
        return `Searching for BankID, it may take a little while …\n
    If a few seconds have passed and still no BankID has been found, you probably don’t have a BankID which can be used for this identification/signing on this computer.\n
     If you have a BankID card, please insert it into your card reader.\n
      If you don’t have a BankID you can get one from your bank.\n
       If you have a BankID on another device you can start the BankID app on that device.`

    else if (status === "failed" && hintCode === "certificateErr")
        return `The BankID you are trying to use is blocked or too old.\n
     Please use another BankID or get a new one from your bank.`

    else if (status === "failed" && hintCode === "startFailed")
        return `Failed to scan the QR code.\n
     Start the BankID app and scan the QR code.\n
      Check that the BankID app is up to date.\n
     If you don't have the BankID app, you need to install it and get a BankID from your bank.\n
      Install the app from your app store or https://install.bankid.com`

    else if (status === "pending")
        return "Identification or signing in progress."

    else if (status === "failed")
        return "Unknown error. Please try again."

    else if (status === "pending" && hintCode === "userMrtd")
        return "Process your machine-readable travel document using the BankID app."
    else if(status === "complete")
        return "Success!"

}