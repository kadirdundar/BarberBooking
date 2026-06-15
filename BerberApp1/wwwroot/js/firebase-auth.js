window.firebaseAuth = {
    recaptchaVerifier: null,
    confirmationResult: null,

    initialize: function (config) {
        if (!config || !config.apiKey) {
            console.warn("Firebase config is missing or invalid. Firebase Phone Auth will not work, running in Demo/Mock Mode.");
            return false;
        }
        
        // Initialize Firebase if not already initialized
        if (firebase.apps.length === 0) {
            firebase.initializeApp(config);
            console.log("Firebase App initialized successfully.");
        }
        return true;
    },

    setupRecaptcha: function (containerId) {
        try {
            if (this.recaptchaVerifier) {
                this.recaptchaVerifier.clear();
                this.recaptchaVerifier = null;
            }

            const element = document.getElementById(containerId);
            if (!element) {
                console.error("reCAPTCHA container element not found:", containerId);
                return false;
            }

            // Create an invisible reCAPTCHA verifier
            this.recaptchaVerifier = new firebase.auth.RecaptchaVerifier(containerId, {
                'size': 'invisible',
                'callback': (response) => {
                    // reCAPTCHA solved, will proceed with signInWithPhoneNumber
                    console.log("reCAPTCHA solved");
                },
                'expired-callback': () => {
                    console.warn("reCAPTCHA expired");
                }
            });
            return true;
        } catch (error) {
            console.error("Error setting up reCAPTCHA verifier:", error);
            return false;
        }
    },

    sendSms: async function (phoneNumber, recaptchaContainerId) {
        try {
            if (!this.recaptchaVerifier) {
                const setupOk = this.setupRecaptcha(recaptchaContainerId);
                if (!setupOk) {
                    throw new Error("Could not setup reCAPTCHA verifier.");
                }
            }

            // Firebase Phone Auth requires E.164 format (e.g. +905XXXXXXXXX).
            // Let's sanitize phone number format if needed or assume caller does it.
            console.log("Sending SMS verification to:", phoneNumber);

            this.confirmationResult = await firebase.auth().signInWithPhoneNumber(phoneNumber, this.recaptchaVerifier);
            console.log("SMS sent successfully.");
            return { success: true, Success: true };
        } catch (error) {
            console.error("Error sending SMS via Firebase:", error);
            // Reset reCAPTCHA on failure to allow retry
            if (this.recaptchaVerifier) {
                this.recaptchaVerifier.clear();
                this.recaptchaVerifier = null;
            }
            return { success: false, Success: false, errorMessage: error.message, ErrorMessage: error.message };
        }
    },

    verifyCode: async function (verificationCode) {
        try {
            if (!this.confirmationResult) {
                throw new Error("No active verification session. Please request SMS code again.");
            }

            const result = await this.confirmationResult.confirm(verificationCode);
            const user = result.user;
            console.log("Phone number verified successfully! Firebase UID:", user.uid);
            return { 
                success: true, 
                Success: true, 
                uid: user.uid, 
                Uid: user.uid, 
                phoneNumber: user.phoneNumber,
                PhoneNumber: user.phoneNumber 
            };
        } catch (error) {
            console.error("Error verifying SMS code:", error);
            return { success: false, Success: false, errorMessage: error.message, ErrorMessage: error.message };
        }
    }
};
