# PollIt Xamarin Mobile Application

PolIt is a mobile application designed to quickly and easily create, share and monitor simple
polls to collect data of opinions. PollIt is available on a variety of operating systems including
Android, iOS, UWP (Universal Windows Platform).

The application was written in C#.NET with the Xamarin framework to enable cross-platform
development for Android, iOS and UWP using a shared codebase. This enabled the application
to be written with more simplicity and less repeated or unnecessary code.

The PollIt mobile client communicates with the PollIt REST API to create, read, edit and delete
polls across many devices linked to a user account. The polls are saved in a Google Firebase
NoSQL database and retrieved by the Node.js server. Node.js and Firebase were chosen
because of their usability, quick setup and security.
