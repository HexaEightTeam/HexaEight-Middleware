# Sample Weather Application .NET Core

### This sample Application uses HexaEight Middleware to provide access to only authorized users using HexaEight Sessions.

**Remember to change the Client ID and Token Server URL in the startup file**

```
services.Add(new ServiceDescriptor(typeof(HexaEightResource), new HexaEightResource("<ClientId>", "<tokenserverURL>")));
```

HexaEight Middleware relies on the following environmental variables to associate the Middleware with a secured hostname. Ensure to set these environment variables before starting the Server

```
HEXA8_RESOURCENAME
HEXA8_LOGINTOKEN
HEXA8_SECRET
HEXA8_EST
HEXA8_APIKEY
```

You can use this tool to [Generate Resource Login Tokens](https://github.com/HexaEightTeam/Generate-HexaEight-Login-Token-For-Resource-Servers) for your environment

