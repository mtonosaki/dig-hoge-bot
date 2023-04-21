# dig-hoge-bot
AzureBotのサンプルです。
単にエコーを返すのと、about と話すと、Bot Frameworkが把握できている情報を返してくれます。

## Azure環境準備
### Azure リソースグループの作成
Azureポータルで、リソースグループを作成します。この例では digbot という名前にしています。  

---
![digtono 2023-04-21 at 09 50 29](https://user-images.githubusercontent.com/34669114/233515524-32f113f2-00b3-4dd5-9d15-a17c0f620620.png)  
---  

### AzureBotの作成
上記リソースグループの中で、AzureBotを新規作成します。  

---
![digtono 2023-04-21 at 09 51 53](https://user-images.githubusercontent.com/34669114/233515714-da3377e9-8f06-4bc9-bc49-2344704f376f.png)  
![digtono 2023-04-21 at 09 54 01](https://user-images.githubusercontent.com/34669114/233516136-1eddf255-68ee-4f32-a7bd-d30c51c3cf6a.png)  
---

### AzureBotのApplication IDを調べる  
Azure Active Directory(AAD)のリソースを開き、App registration を開くと、先ほど AzureBotの名前で "Application"が登録されているのが見えますので、それをクリックして開きます。  

---
![digtono 2023-04-21 at 09 58 40](https://user-images.githubusercontent.com/34669114/233516394-d96beb06-a1ca-425e-bc45-89861db3e65f.png)  
---

そして、Application (client) IDを コピーして覚えておきます。

---
![digtono 2023-04-21 at 10 09 02](https://user-images.githubusercontent.com/34669114/233519491-c47a59f8-7dc0-4949-872f-1783454569ea.png)  
---

### AzureBotのApplication Secretを再発行する  
AzureBotが自動的に作成したシークレットは確認できなくなっているので、Azure CLIを使って再作成します。  

![digtono 2023-04-21 at 10 38 28](https://user-images.githubusercontent.com/34669114/233520754-ebc579be-2324-4195-abc6-1bc3e24b1c3b.png)  
---  
`az ad app credential reset --id <Application (cliend) IDを入力>`  

すると以下のように新しいシークレットが表示されますので、覚えておきます。（取り扱い注意のシークレットです）  

![digtono 2023-04-21 at 10 43 57](https://user-images.githubusercontent.com/34669114/233521320-63641c3d-04dd-48e0-8603-c41da0a56d77.png)  
---

### Webサーバーを作成する  
Botのバックエンドを動作させるWebサーバーをAzureに建てます。  具体的には、 AppServicePlanと、AppServiceのリソース追加になります。  
まず、最初に作成したリソースグループの中で、WebApp を新規作成します。  
![digtono 2023-04-21 at 10 54 45](https://user-images.githubusercontent.com/34669114/233522493-24c7133b-cdba-477b-a025-4c5243804a70.png)  
![digtono 2023-04-21 at 10 58 12](https://user-images.githubusercontent.com/34669114/233522729-9bc3ef52-d5b8-427c-81ec-6f2b184f83b5.png)  

WebAppを新規作成すると、次の画面で仕様を入力します。    

---  
![digtono 2023-04-21 at 11 00 11](https://user-images.githubusercontent.com/34669114/233523177-fea62d68-a475-4097-99c6-e2bd5ffda7c6.png)
![digtono 2023-04-21 at 11 02 22](https://user-images.githubusercontent.com/34669114/233523436-b32bc479-215f-41eb-9e3d-7678d1d7689d.png)
![digtono 2023-04-21 at 11 04 43](https://user-images.githubusercontent.com/34669114/233523574-8e4ff3c9-ac3e-4140-8b96-0b962fad50a0.png)
![digtono 2023-04-21 at 11 05 31](https://user-images.githubusercontent.com/34669114/233523633-9802941e-b355-4100-91b8-2bc294a179c1.png)
![digtono 2023-04-21 at 11 05 52](https://user-images.githubusercontent.com/34669114/233523664-b76b4e96-91f7-466d-92f5-ceb592daadbb.png)
---  



