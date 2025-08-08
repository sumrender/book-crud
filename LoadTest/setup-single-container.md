Create container

```
az container create \
  --resource-group load-test-rg \
  --name k6-10k-test \
  --image grafana/k6:latest \
  --cpu 4 \
  --memory 8 \
  --os-type Linux \
  --command-line "k6 run /scripts/20k-users.js" \
  --restart-policy Never \
  --azure-file-volume-account-name loadteststoragerg \
  --azure-file-volume-account-key ACCOUNT_KEY \
  --azure-file-volume-share-name k6-scripts \
  --azure-file-volume-mount-path /scripts
```


Follow logs
```
az container logs --resource-group load-test-rg --name k6-10k-test --follow
```


Local
```
k6 run tj-10k-10m.js 
```