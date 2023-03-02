## About
This is a template to build your own launcher regarding the bsgo 2.0 ressurection project.

## Concept
- Launcher builds up a connection with my loginserver and sends the required login credentials.
- Loginserver sends a session to you and the gameserver.
- Loginserver sends the gameserver ip your game client has to connect to aswell
- Launcher should startup the client by launch-parameters.
- Game client sends the session to the gameserver and gameserver verifies if you have a valid session and logs you into you account

## why
- I'm not interested in building windows applications
- Feel free to write your own launcher.