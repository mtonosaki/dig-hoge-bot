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

### WebApp に、AzureBotの資格情報を紐付ける  

作成したWebAppを開き、さらに Configulationの画面から、環境変数を入力します。  

---  
![digtono 2023-04-21 at 11 07 54](https://user-images.githubusercontent.com/34669114/233524163-4fc2d6bb-b28e-462f-a63f-c796ded7cbfc.png)
---  

|  Name  |  Value  |
| ---- | ---- |
|  MicrosoftAppId  |  `Application (cliend) ID`  |
|  MicrosoftAppPassword  |  `先ほど再作成したシークレット`  |
|  MicrosoftAppTenantId  |    |
|  MicrosoftAppType  |  `MultiTenant`  |

今回はステージング環境は作らないので、Deployment Slot Settingは、OFFにします。  
すべて入力し終わったら、Save ボタンを押して、保存してください。  

### WebAppのURLを調べる

![digtono 2023-04-21 at 11 17 19](https://user-images.githubusercontent.com/34669114/233525302-f72cd650-2ecb-479a-ac90-6c4cfc49affd.png)


### AzureBotに、WebAppのエンドポイントを紐付ける

AzureBotを開き、Configulationで Messaging Endpointを入力する。

---  
![digtono 2023-04-21 at 11 20 48](https://user-images.githubusercontent.com/34669114/233525925-a56646f6-234a-407a-aef0-48754f0723c5.png)  
---   

以上で、Azureの環境構築は完了しました。  

## 配信  

Gitクローンした資源から、Visual Studioを起動する。
その後、プロジェクト名を右クリックして、Publish... を実行する。  

---  
![digtono 2023-04-21 at 11 31 33](https://user-images.githubusercontent.com/34669114/233526855-9bd54635-8663-4b3f-b8a4-280950420ea1.png)  
---  

ターゲットは Azure→Azure App Service（Windows） を選択  

---  
![digtono 2023-04-21 at 11 34 04](https://user-images.githubusercontent.com/34669114/233527027-0a3fc33d-cdff-4f6d-859e-2e124e2b234d.png)  
![digtono 2023-04-21 at 11 35 21](https://user-images.githubusercontent.com/34669114/233527220-32b8cbbd-e3e0-45c6-81dd-15e87b079bca.png)  
--- 

先ほど作成した WebApp を選択して Nextを押し、続けて 配信設定を保存します。

---  
![image](https://user-images.githubusercontent.com/34669114/234793433-b78e0bc7-c70b-450f-ade4-af7076d3c921.png)
![digtono 2023-04-21 at 11 38 10](https://user-images.githubusercontent.com/34669114/233527525-c9bd32e6-bfb9-4fae-8c2f-4c2adc68a6a6.png)
---  

この画面で、Publishボタンを押すと、ボットのバックエンドが Azureにデプロイされます。

---
![digtono 2023-04-21 at 11 39 53](https://user-images.githubusercontent.com/34669114/233527771-01a63b35-aa71-4515-9a82-5a846efc2cf4.png)  
---

## 動作確認  

AzureBotの画面でテストできます。

![digtono 2023-04-21 at 11 43 27](https://user-images.githubusercontent.com/34669114/233528549-46032d84-835a-4fc3-ae11-c9717ea39d4b.png)




