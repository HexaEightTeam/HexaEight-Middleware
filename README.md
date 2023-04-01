# HexaEight-Middleware
HexaEight Middlewares uses HexaEight Token Server to provide protection for your API by implementing token-less authentication provided by HexaEight Sessions

HexaEight Middleware intercepts every request and examines if the request is authenticated, if the request is an authenticated request, it populates the Http Context with an identity claim like shown below with all the information required for the Server Application.

```
Sender : Contains the email address of the sender
Receiver : Contains your own resource name for verification purposees
ID : Unique ID per Client Application
RequestTime : Unix Timestamp when the request was sent
RequestRcvd : Unix Timestamp when the request was received (This is automatically computed by the Middleware)
MessageVerifier : The message verifier that is used for validating PKCE requests
RequestHash : A unique hash of the request, can be used to deteect replay transactions, like for example a Card purchase txn.
OriginHash : This contains the hash of the Client Application which sent the request, (Note : This cannot be spoofed)
Scope : The Scope/Role which was used to login to the Client Application
PkceVerified : If true, then this entire message has been verified and it can safely be consumed by the pipeline
Provider : HexaEight - This is a constant which can be used to verify if the incoming request is an HexaEight Session.
```

** Note: The developer/implementor of HexaEight Middleware must remember that the request from HexaEight Session inside the Client Application will reach the Middleware API Server only if HexaEight Token Server authorizes the request and issues a Valid Client Token for the Middleware API Servrer. Hence there is no specific authorization logic implemented inside the Middleware code. It is up to the developer/implementor to add additional authorization if required for the application as per their requirements**

Below is a sample request url captured by the Middleware session. Note that the entire URL is encrypted by HexaEight Client Session before it is inspected by HexaEight Middleware for consumption.

```
info: Microsoft.AspNetCore.Hosting.Diagnostics[2]
Request finished HTTP/1.1 GET http://localhost:5000/DY8YEf6N6oV6EylRf07!2BEroXnmTCQuYHnleCb2KN!2FiSqRvU7yUmJDD4zvwECeH1wU0p7Q9Utt3A3UKOB!2BypfFU!2BoPQU6B4R9!2FYJwPJdtJnsqPIF5UmTOLjMBUEmaF1RI7lZxLQqKFwUbWw1zQFVqFNAJuoxxXC9fnFQadDMpDe2JqJ8zW1xIhCMixYlKDupPdh!2FgXkszbS24hX5Am1JaZDhOTXoIPBdckxZ0hUkHnEzKDjwTBo3RFt5IMwY6g!2BI6iqhvB2V470h!2FEWghwRSdTch4UY8rYjmAEmquK5BSKW0iKlgnDW!2BUE3JNkV6vYZsSzzZAX4M7i4pFfAQ4igWutl!2BoKG!2F2bCxYmhaoTG8N81BsTqCLnA2wfwRs0BBASpt4nEOtWiQ!2Fwg6IYeEIUmu0VwBrd40OR8k6kHM7XPcT9EeTqA!3D!3D!7CqYdmLrpEy1WYIWBtE4M1gUJbwAtEQCtLWlNqW6Q7M1mad282q3kIfYImUxxWO5RZs0CZV3lHSQwPYmlg!2B9!2F0S2otYXddf1hFgWpVKxUV7QbEDZFD8nd6aSBh7x0jCH8WiGcpdDEEaIZpFm4sZEziCQkeznDFPhQ7!2BjEEQgHVaOh2Lo1hTjJPARWAb3VSgRM0whuNhYsI3mgEEGUf0jb3ESgBzUfRNN04yyCaGsNdOhpcVdFfhGkXCTAms15S7wsDay4iK9cuwCqVNBMnBFEcN78a6B8EOys6TiN3ZvplIjtGQulqYFkYfk9FNBtXclAe5HRHFkZEHCSUJXMjBIdLdwkuShyteXwmx4LxZspk1xGLW40S5VUbXoI5gABDcVwkV1RYEPkq9EIiD8IFBnQSEB1IgyLmP4ZhwFysUbEJPQ8VLvwfAEPZLGJDNlaAVShzexqwhFOEKoRxdOxvjF84eYln!2B3!2BBQ0Ag2knvXw0NNlfXOmGDqi8vbxN5G3eVg3pJBy8ISTlsRhkafys9AAMqXNF3uSXLDDlVlFnWgjlCVSspStUfH31ufYALBGvOE59Y8QvWPoxUB0kMAQEFEr8H0b0t2km9J7RtoCiCAqIoqV94gyIn6VJ4ItE6akiEKCJ!2FK34rTiBLAVQPQO9SEwTVeasknkv8JXxPdx2se7K7s!2FK!2FLYMj3z!2FYESJ8qQ1UZdkebXniUHxRVDpYiJI!2FoHdrRWRSsEHYAQd00g1tgWk7cGD4ESMH3ngcF8yDe0b1Hojjly0!3D - - - 200 - application/octet-stream 3984.0168ms
```

Below are the Server side Demo code samples which can be used to protect APIs

Sample-Weather-Application (.NET Core) - Written in c#




