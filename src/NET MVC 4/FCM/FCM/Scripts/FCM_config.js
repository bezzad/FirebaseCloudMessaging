//if ('serviceWorker' in navigator) {
//    navigator.serviceWorker.register('scripts/firebase-messaging-sw.js').then(function (registration) {
//        console.log('ServiceWorker registration successful with scope: ', registration.scope);
//    }).catch(function (err) {
//        //registration failed :(
//        console.log('ServiceWorker registration failed: ', err);
//    });
//} else {
//    console.log('No service-worker on this browser');
//}

// Initialize Firebase
var config = {
    //apiKey: "AIzaSyCZ4FSUaAWrbmcirHaBnYvmDnTaQjwTxJ4",
    //authDomain: "test-76fde.firebaseapp.com",
    //databaseURL: "https://test-76fde.firebaseio.com",
    //projectId: "test-76fde",
    //storageBucket: "test-76fde.appspot.com",
    messagingSenderId: "710554227089"
};
firebase.initializeApp(config);

// [START get_messaging_object]
// Retrieve Firebase Messaging object.
const messaging = firebase.messaging();
// [END get_messaging_object]

// IDs of divs that display Instance ID token UI or request permission UI.
const tokenDivId = 'token_div';
const permissionDivId = 'permission_div';
var token = null;

// [START refresh_token]
// Callback fired if Instance ID token is updated.
messaging.onTokenRefresh(function () {
    messaging.getToken()
        .then(function (refreshedToken) {
            console.log('Token refreshed.');
            // Indicate that the new Instance ID token has not yet been sent to the
            // app server.
            setTokenSentToServer(false);
            // Send Instance ID token to app server.
            sendTokenToServer(refreshedToken);
            // [START_EXCLUDE]
            // Display new Instance ID token and clear UI of all previous messages.
            resetUI();
            // [END_EXCLUDE]
        })
        .catch(function (err) {
            console.log('Unable to retrieve refreshed token ', err);
            showToken('Unable to retrieve refreshed token ', err);
        });
});
// [END refresh_token]

// [START receive_message]
// Handle incoming messages. Called when:
// - a message is received while the app has focus
// - the user clicks on an app notification created by a sevice worker
//   `messaging.setBackgroundMessageHandler` handler.
messaging.onMessage(function (payload) {
    console.log("Message received. ", payload);
    // [START_EXCLUDE]
    // Update the UI to include the received message.
    appendMessage(payload);
    // [END_EXCLUDE]
});
// [END receive_message]

function resetUI() {
    clearMessages();
    showToken('loading...');
    // [START get_token]
    // Get Instance ID token. Initially this makes a network call, once retrieved
    // subsequent calls to getToken will return from cache.
    messaging.getToken()
        .then(function (currentToken) {
            if (currentToken) {
                console.log("token:   " + currentToken);
                sendTokenToServer(currentToken);
                updateUIForPushEnabled(currentToken);
            } else {
                // Show permission request.
                console.log('No Instance ID token available. Request permission to generate one.');
                // Show permission UI.
                updateUIForPushPermissionRequired();
                setTokenSentToServer(false);
            }
        })
        .catch(function (err) {
            console.log('An error occurred while retrieving token. ', err);
            showToken('Error retrieving Instance ID token. ', err);
            setTokenSentToServer(false);
        });
}
// [END get_token]

function showToken(currentToken) {
    // Show token in console and UI.
    var tokenElement = document.querySelector('#token');
    tokenElement.textContent = currentToken;
}

// Send the Instance ID token your application server, so that it can:
// - send messages back to this app
// - subscribe/unsubscribe the token from topics
function sendTokenToServer(currentToken) {
    if (!isTokenSentToServer()) {
        console.log('Sending token to server...');
        token = currentToken; // TODO(developer): Send the current token to your server.
        $.post(window.origin + '/home/StoreToken', { token: currentToken });
        setTokenSentToServer(true);
    } else {
        console.log('Token already sent to server so won\'t send it again ' +
            'unless it changes');
    }

}

function isTokenSentToServer() {
    return window.localStorage.getItem('sentToServer') === 1;
}

function setTokenSentToServer(sent) {
    window.localStorage.setItem('sentToServer', sent ? 1 : 0);
}

function showHideDiv(divId, show) {
    const div = document.querySelector('#' + divId);
    if (show) {
        div.style = "display: visible";
    } else {
        div.style = "display: none";
    }
}

function requestPermission() {
    console.log('Requesting permission...');
    // [START request_permission]
    messaging.requestPermission()
        .then(function () {
            console.log('Notification permission granted.');
            // TODO(developer): Retrieve an Instance ID token for use with FCM.
            // [START_EXCLUDE]
            // In many cases once an app has been granted notification permission, it
            // should update its UI reflecting this.
            resetUI();
            // [END_EXCLUDE]
        })
        .catch(function (err) {
            console.log('Unable to get permission to notify.', err);
        });
    // [END request_permission]
}

function deleteToken() {
    // Delete Instance ID token.
    // [START delete_token]
    messaging.getToken()
        .then(function (currentToken) {
            messaging.deleteToken(currentToken)
                .then(function () {
                    console.log('Token deleted.');
                    setTokenSentToServer(false);
                    // [START_EXCLUDE]
                    // Once token is deleted update UI.
                    resetUI();
                    // [END_EXCLUDE]
                })
                .catch(function (err) {
                    console.log('Unable to delete token. ', err);
                });
            // [END delete_token]
        })
        .catch(function (err) {
            console.log('Error retrieving Instance ID token. ', err);
            showToken('Error retrieving Instance ID token. ', err);
        });

}

// Add a message to the messages element.
function appendMessage(payload) {
    const messagesElement = document.querySelector('#messages');
    const dataHeaderELement = document.createElement('h5');
    const dataElement = document.createElement('pre');
    dataElement.style = 'overflow-x:hidden;';
    dataHeaderELement.textContent = 'Received message:';
    dataElement.textContent = JSON.stringify(payload, null, 2);
    messagesElement.appendChild(dataHeaderELement);
    messagesElement.appendChild(dataElement);
}

// Clear the messages element of all children.
function clearMessages() {
    const messagesElement = document.querySelector('#messages');
    while (messagesElement.hasChildNodes()) {
        messagesElement.removeChild(messagesElement.lastChild);
    }
}

function updateUIForPushEnabled(currentToken) {
    showHideDiv(tokenDivId, true);
    showHideDiv(permissionDivId, false);
    showToken(currentToken);
}

function updateUIForPushPermissionRequired() {
    showHideDiv(tokenDivId, false);
    showHideDiv(permissionDivId, true);
}

resetUI();