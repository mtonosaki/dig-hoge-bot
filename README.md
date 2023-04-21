# dig-hoge-bot
AzureBotのサンプルです。
単にエコーを返すのと、about と話すと、Bot Frameworkが把握できている情報を返してくれます。

## 開発準備
### Azure リソースグループの作成
Azureポータルで、リソースグループを作成します。この例では digbot という名前にしています。  
![digtono 2023-04-21 at 09 50 29](https://user-images.githubusercontent.com/34669114/233515524-32f113f2-00b3-4dd5-9d15-a17c0f620620.png)  

### AzureBotの作成
上記リソースグループの中で、AzureBotを新規作成します。  
![digtono 2023-04-21 at 09 51 53](https://user-images.githubusercontent.com/34669114/233515714-da3377e9-8f06-4bc9-bc49-2344704f376f.png)  
![digtono 2023-04-21 at 09 54 01](https://user-images.githubusercontent.com/34669114/233516136-1eddf255-68ee-4f32-a7bd-d30c51c3cf6a.png)  

### AzureBotのIDとキーを調べる
Azure Active Directory(AAD)のリソースを開き、App registration を開くと、先ほど AzureBotの名前で "Application"が登録されているのが見えますので、それをクリックして開きます。  
![digtono 2023-04-21 at 09 58 40](https://user-images.githubusercontent.com/34669114/233516394-d96beb06-a1ca-425e-bc45-89861db3e65f.png)  

そして、Application (client) IDを コピーして覚えておきます。
![digtono 2023-04-21 at 10 02 14](https://user-images.githubusercontent.com/34669114/233516884-335ef5a9-04ba-4fbd-9462-34c52448ae31.png)  

