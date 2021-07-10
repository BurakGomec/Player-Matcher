# Player-Matcher
A Rest API for finding the best player to player match in a multiplayer game's dataset with kNN. </br>

## Environment
- The Rest API was developed with .NET Core 5.
- MongoDB was used as database.

## Architecture
The service has two model and two controller classes. </br>

### Models
- **Account** : This class keeps user's authentication properties (id, e-mail, username, password). 
- **Player** : In addition to the Account class, It keeps user's in-game properties and stats (status, level).

### Controllers
- **Auth** : This controller does authentication transactions (sign-in, sign-up, logout, delete).
- **GameScene** : This controller is for in-game actions (find-opponent and level-up).
For to use "logout", "delete", "find-opponent", "level-up" methods client have to send a specific token.

## How to Develop/Test?
First of all, create a MongoDB database and receive a connection string. After that, put this password to the API's Constants class' connectionInfo field.

## Endpoints/Requests
### Auth Endpoints
- /api/v1/Auth/signup (username, password, email) [POST]
- /api/v1/Auth/signin (username, password, email) [POST]
- /api/v1/Auth/logout (username, token) [POST]
- /api/v1/Auth/delete (username, token) [POST]
### GameScene Endpoints
- /api/v1/Auth/matchmaking (username, token) [GET]
- /api/v1/Auth/levelup (username, token) [PATCH]

## Client Connection
A native iOS application was used to test the API. You can reach the repo from [here](https://github.com/BurakGomec/Player-Matcher-iOS-App). </br>

## The Client App's Preview
<img src="https://github.com/BurakGomec/Player-Matcher-iOS-App/blob/main/screen.gif" width="25%" height="25%"/>

