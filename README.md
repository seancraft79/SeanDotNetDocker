# SeanDotNetDocker

### Install Docker in Ubuntu

##### 한줄씩 차례대로 입력

```
sudo apt update

sudo apt install apt-transport-https ca-certificates curl software-properties-common

curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo apt-key add -

sudo add-apt-repository "deb [arch=amd64] https://download.docker.com/linux/ubuntu bionic stable"

sudo apt update

apt-cache policy docker-ce
```

##### 마지막 줄 입력하고 나면 다음 메세지
```
docker-ce:
  Installed: (none)
  Candidate: 18.06.1~ce~3-0~ubuntu
  Version table:
     18.06.1~ce~3-0~ubuntu 500
        500 https://download.docker.com/linux/ubuntu bionic/stable amd64 Packages
     18.06.0~ce~3-0~ubuntu 500
        500 https://download.docker.com/linux/ubuntu bionic/stable amd64 Packages
     18.03.1~ce~3-0~ubuntu 500
        500 https://download.docker.com/linux/ubuntu bionic/stable amd64 Packages
```

### Docker 설치
```
sudo apt install docker-ce
```

##### 정상적으로 작동하는지 확인
```
sudo systemctl status docker
```


### Docker-compose 설치
##### 설치여부 확인
```
docker-compose -v
```

##### docker-compose 설치
```
apt install docker-compose
```

### Install Git in server
##### Install Git
```
sudo apt update

sudo apt install git
```

##### check git version
```
git --version
```

##### Clone source
```
git clone https://github.com/seancraft79/SeanDotNetDocker.git
```

### Build docker compose
##### Staging
```
docker-compose -f docker-compose.yml -f docker-compose.debug.yml up --build -d
```

##### Production
```
docker-compose -f docker-compose.yml -f docker-compose.prod.yml up --build -d
```
