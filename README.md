* It's korean.(한국어입니다)<br>
* ascom-standards.org 에서 작성된 코드와 별도로 표기된 원작자의 코드를 제외한, 크게 가치는 없지만 제가 직접 작성한 모든 코드와 데이터를 공유합니다.<br>
* 단, 이곳의 ASCOM/Focuser 과 ASCOM/HW 이하 관련 자료만 해당됩니다.<br>
* 또한 이 곳의 자료를 사용함으로 인해 어떠한 문제가 발생되어도 책임질 수 없음을 명백히 알려드립니다.<br>

본 제작품은 ASCOM ORG의 개발자 구성요소를 다운로드로 제공받아 작성했습니다.<br>
개발환경은 VS2022와 C#을 사용하고 ASCOM에서 제공한 템플릿을 사용했습니다.<br>
마이크로 프로세서는 Microchip의 DSPIC33FJ12MC201-I/SS, 컴파일러는 XC16을 통신칩은 FT230X을 사용했습니다.<br>

나는 몇 달전에 C#을 처음 접했으며 아직도 프로그래밍 언어에 익숙치 못해 구글이 없으면 C# 코딩을 하나도 할 수 없음을 알려드립니다.<br>
그러므로 여기에 사용된 대부분의 코드는 인터넷에서 검색한 자료를 짜깁기 했음을 부끄럽지만 밝힙니다. 원작자 께서는 부디 양해해 주시기 바랍니다.<br>

![ascom site](https://github.com/vbred-naver/ASCOM/assets/170990692/e1918ae9-79cd-44f5-b493-dfcb64875500)
![define](https://github.com/vbred-naver/ASCOM/assets/170990692/ea4dec91-cafb-441c-b993-a44d294e807b)
![driver](https://github.com/vbred-naver/ASCOM/assets/170990692/d5f2a374-adda-4652-bef9-e8176baf98a6)
![ftprog](https://github.com/vbred-naver/ASCOM/assets/170990692/ca84a7a7-ea83-4e61-b350-dae428079efa)

![ftdi_selset](https://github.com/vbred-naver/ASCOM/assets/170990692/33b25f55-72c5-4008-a4fc-48e85ae08466)
![xc16](https://github.com/vbred-naver/ASCOM/assets/170990692/00949cfe-7fec-4dcc-8294-86ec320f0ad0)

![sch](https://github.com/vbred-naver/ASCOM/assets/170990692/b054edcb-583d-4ec1-80e4-376913ed466a)

![motor](https://github.com/vbred-naver/ASCOM/assets/170990692/ca8231fd-8604-4461-a9d8-cb023dc1dcbf)
![motor spec](https://github.com/vbred-naver/ASCOM/assets/170990692/ebd71582-06ac-4cf2-89ed-8c9d85e64a4b)

![pcb assy top](https://github.com/vbred-naver/ASCOM/assets/170990692/3f22eb16-4bf9-44a0-bec9-d359a51f1b3d)
![pcb assy bottom](https://github.com/vbred-naver/ASCOM/assets/170990692/f93f81bc-6a88-4afc-98c3-12b98556bbf1)

![bracket![dwg1](https://github.com/vbred-naver/ASCOM/assets/170990692/9131f7ec-c466-4c94-91be-c51fbfad48d9)
![dwg5](https://github.com/vbred-naver/ASCOM/assets/170990692/77b5dccd-4651-4064-a8b9-f34c715d91dd)

1](https://github.com/vbred-naver/ASCOM/assets/170990692/2bb446c3-ad36-4236-986f-2bde1402e50b)
![set1](https://github.com/vbred-naver/ASCOM/assets/170990692/a5bb84db-dcec-4487-b9ba-7ae66da71860)
![set2](https://github.com/vbred-naver/ASCOM/assets/170990692/bed26120-0a8e-4c08-9611-f01b4adc252d)
![set3](https://github.com/vbred-naver/ASCOM/assets/170990692/e64a1083-0b6d-48ef-a1c5-656af2032752)
![nina fo1](https://github.com/vbred-naver/ASCOM/assets/170990692/4dfb5d7b-721c-47a6-bb03-e2b3fcd7728b)


다음의 포커서 관련 자료로 blog를 참조해 주세요.<br>
ASCOM 지원 Auto Focuser(EAF) 제작 - 1. 드라이버(소프트웨어) : https://blog.naver.com/vbred/223416366278<br>
ASCOM 지원 Auto Focuser(EAF) 제작 - 2. 펌웨어(dsPIC33) : https://blog.naver.com/vbred/223417237001<br>
ASCOM 지원 Auto Focuser(EAF) 제작 - 3. 회로도 : https://blog.naver.com/vbred/223419394972<br>
ASCOM 지원 Auto Focuser(EAF) 제작 - 4. PCB : https://blog.naver.com/vbred/223420359816<br>
ASCOM 지원 Auto Focuser(EAF) 제작 - 5. 케이스 : https://blog.naver.com/vbred/223442445757<br>
ASCOM 지원 Auto Focuser(EAF) 제작 - 6. 첫 번째 시험촬영 : https://blog.naver.com/vbred/223448779931<br>

