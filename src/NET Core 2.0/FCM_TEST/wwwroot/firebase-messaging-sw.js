// // Import and configure the Firebase SDK
// // These scripts are made available when the app is served or deployed on Firebase Hosting
// // If you do not serve/host your project using Firebase Hosting see https://firebase.google.com/docs/web/setup
//importScripts('https://www.gstatic.com/firebasejs/4.5.1/firebase-app.js');
//importScripts('https://www.gstatic.com/firebasejs/4.5.1/firebase-messaging.js');

// const messaging = firebase.messaging();

/**
 * Here is is the code snippet to initialize Firebase Messaging in the Service
 * Worker when your app is not hosted on Firebase Hosting.
 */
// [START initialize_firebase_in_sw]
// Give the service worker access to Firebase Messaging.
// Note that you can only use Firebase Messaging here, other Firebase libraries
// are not available in the service worker.
importScripts("https://www.gstatic.com/firebasejs/4.5.2/firebase-app.js");
importScripts("https://www.gstatic.com/firebasejs/4.5.2/firebase-messaging.js");
//importScripts('https://www.gstatic.com/firebasejs/4.5.2/firebase.js');

// Initialize the Firebase app in the service worker by passing in the
var config = {
    //apiKey: "AIzaSyCZ4FSUaAWrbmcirHaBnYvmDnTaQjwTxJ4",
    //authDomain: "test-76fde.firebaseapp.com",
    //databaseURL: "https://test-76fde.firebaseio.com",
    //projectId: "test-76fde",
    //storageBucket: "test-76fde.appspot.com",
    messagingSenderId: "710554227089"
};
firebase.initializeApp(config);

// Retrieve an instance of Firebase Messaging so that it can handle background
// messages.
const messaging = firebase.messaging();
// [END initialize_firebase_in_sw]

// If you would like to customize notifications that are received in the
// background (Web app is closed or not in browser focus) then you should
// implement this optional method.
// [START background_handler]
messaging.setBackgroundMessageHandler(function(payload) {
    console.log("[firebase-messaging-sw.js] Received background message ", payload);
    // Customize notification here
    const notificationTitle = "Background Message Title";
    const notificationOptions = {
        body: "Background Message body. " + payload.data.status,
        icon: "https://taaghche.ir/assets/images/taaghchebrand.png",
        dir: "rtl"
    };

    return self.registration.showNotification(notificationTitle, notificationOptions);
});
// [END background_handler]