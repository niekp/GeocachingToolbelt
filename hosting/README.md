# Publish
Publish the .NET project

# Sync to server
Use rsync or any other tool to sync the published app to the server.

# Service
Place this service in `/etc/systemd/system/geocachingtoolbelt.service`

```
[Unit]
Description=Geocache toolbelt

[Service]
WorkingDirectory=/opt/GeocachingToolbelt
ExecStart=/usr/bin/dotnet /opt/GeocachingToolbelt/GeocachingToolbelt.dll
Restart=always
# Restart service after 10 seconds if the dotnet service crashes:
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=geocachingtoolbelt
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=Production

[Install]
WantedBy=multi-user.target
```

Enable and start the service.
`sudo systemctl enable geocachingtoolbelt` and `sudo service geocachingtoolbelt start`

# Deploy script
The deploy script in this folder syncs and restarts the service.

