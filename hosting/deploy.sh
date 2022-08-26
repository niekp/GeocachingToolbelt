ssh -t ovh "sudo service geocachingtoolbelt stop && sudo chown -R niek /opt/GeocachingToolbelt"
rsync -avz --exclude wwwroot/GPX/* GeocachingToolbelt/bin/Release/netcoreapp3.0/publish/* ovh:/opt/GeocachingToolbelt/
ssh -t ovh "sudo chown -R www-data /opt/GeocachingToolbelt && sudo service geocachingtoolbelt start"
