## 프로젝트 목적
내일배움 캠프 과제 진행, 3D 개발 연습 공간입니다. </br>
어떤 것들을 의도했고, 새로운 지식을 알아야 풀 수 있었던 것들을 여기 기록해둡니다.

## Mathf.LerpAngle
마우스를 움직여 고개를 돌릴 때 뚝뚝 끊기는 느낌이 커서의 픽셀값을 받아 움직이면 그럴 수 있겠다 싶어서</br>
Lerp로 부드럽게 이동을 하도록 처리할 때 생긴 문제였습니다.</br>
359도에서 2도를 더하면 자연스럽게 해당 프레임에 1도를 보여주지만</br>
Lerp를 적용하면 359에서 2도를 움직이지 않고 역방향으로 358도를 움직였습니다.</br>
해결 로직을 직접 구상해내지는 못했지만</br>
LerpAngle이 360 경계를 놓고 361과 1 두 경우의 수를 다 계산해서 가까운 쪽으로 움직이는 방법이라는 걸 배워두었습니다.</br>
거기에 Vector3 통째로 LerpAngle을 지원하는 건 따로 없어서 축별로 LerpAngle을 걸어 시선처리를 구현했습니다. 

![image](https://github.com/user-attachments/assets/cb35ed53-c81e-497a-b695-b3f458eba04c)</br>
PlayerController.cs



## 굴러다니는 플레이어 구체
### 지면을 뚫고 내려가는 구체
지면이 Rigidbody를 갖고 있을 때 질량을 높이니 뚫지 않게 되었습니다.</br>
나중에 지면의 질량으로 이걸 처리하는 건 정석이 아닌 것 같아서</br>
RigidBody는 제거했고, isKinematic으로 처리하고 있습니다. </br>


### 구르는 걸 멈추지 않던 문제, 주는 힘을 무시하는 속도 상한 문제
Physics Material이라는 게 있다는 건 알고 있었어서</br>
구체와 지면에 모두 적용했고 마찰력을 상당히 높여도 구체가 계속 굴렀습니다.</br>
지면에 Rigidbody도 넣어보고, 그래서 위의 지면 뚫는 문제도 발생하고</br>
3을 넘는 수치를 웃돌던 friction 수치도 정석이 아닌 것 같아서 과정 중에 1 이하로 돌려놨는데</br>
무슨 변화가 되게 했는지는 잘 모르겠지만 구체가 자연히 구르다가 멈추게 되었습니다.</br>

이 문제는 다른 문제를 다루면서 풀렸습니다.</br>
분명 AddForce인데 **MovePower를 5로 하나 95로 하나 속도의 상한이 3.5~3.8정도의 변화만 보였다가** </br>
**99가 되는 순간 리미터가 풀린 것처럼 움직이는 현상**이 있었습니다.</br>
원인은 피직스 매테리얼의 **정지 마찰** 이었습니다. (나중에 동적 마찰도 영향이 있음을 확인했습니다.) </br>
셋팅되었던 질량 10과 수직항력(중력) 9.8의 곱, 그리고</br>
피직스 매테리얼의 정지마찰 계수를 1로 놓았었기 때문에 전부 곱해서</br>
약 98을 넘겨야 움직이는 것이 이치에 맞던 것이었습니다.</br>
정지 마찰을 의심하지 못했던 이유는 속도 3.5 - 3.8로 움직이기는 했기 때문이고</br>
어쨌든 이 부분의 삽질을 반복하는 와중에 다룬 값 중에서 drag라는 값에 의해</br>
구체가 스스로 구르다가 멈추는 것을 볼 수 있었습니다.</br>
앞부분에서 분명 눈에 띄어서 한번 다룬 것 같은데 이걸 효과가 없다고 판단했었는지는 잘 모르겠지만</br>
구체조차 표면의 마찰이 일어나는 현실의 물리를 그래픽 공간에서 그대로 할 수가 없으니</br>
그냥 자기 의지로 운동량 감소를 의도하는 것이 drag다 하고</br>
애매하게 알고 있었던 부분에 대해 명확한 정리를 할 수 있었습니다.</br>
</br>
아직 속도 3.5~3.8로는 왜 움직였는가 하는 문제가 남아있는데</br>
이 부분은 정지마찰력 내에서 주어지는 힘을 카운터치는 힘도 만들어내야 하는데</br>
이 부분의 로직과 관련됐을 것으로 추측합니다. 


## 상호작용 근거리 탐색 못 하는 문제
레이캐스트로는 화면 정중앙에 탐색하고자하는 사물의 콜라이더가 들어와야 하는데</br>
**완전 정중앙이 아니어도 잡히는 후한 판정이기를 바라서** 그 방법을 찾았고,</br>
SphereCast를 적용하였습니다.</br>
탐지 반경이 좋아지긴 했는데, 너무 가까우면 물체를 인지하지 못했습니다.</br>
스피어가 이미 생성된 지점에 걸쳐있는 오브젝트는 인지 못 하도록 되어있어보입니다.</br>
해결 아이디어가 없지는 않은데, 레이는 출발지점과 방향으로 이루어져 있으니까</br>
방향을 camera.transform.forward로 유지하고 출발 지점을 역방향으로 반지름만큼 빼는 것입니다.</br>
강의에서도 지면 탐지를 위해 박스캐스트를 할 때 레이 출발 지점을 약간 위로 들어올리고 밑으로 쐈으니까요.</br>
다만 아이디어를 떠올리고서는 다시 생각해보니 현실적으로도 너무 코앞에 있는 물체보다</br>
양쪽 눈으로 보기 좋은 거리만큼 떨어져있는 편이 잡기 좋기도 하고</br>
생각보다 스피어 캐스트의 반경을 줄여도 후했기 때문에</br>
적당한 값조절로 마무리한 상태입니다.</br>
</br>
후에 3인칭 카메라를 개발하고 지면 충돌을 트리거를 썼다가 다시 레이캐스트로 돌리면서</br>
레이의 출발점을 뒤로 살짝 빼는 기본 방식을 적용해 지금은 대략 원하는 느낌으로 탐지가 잘 되고 있습니다.</br>
3인칭으로 해도 물체 탐지는 플레이어로부터 출발해야 하다보니</br>
고개를 들면 탐지되던 물체가 3인칭에선 카메라가 지면을 뚫고 내려가야 해서</br>
3인칭 시 스피어 캐스트 지름을 좀 더 후하게 처리하고 있습니다.</br>

## 탐지 외곽선
필수 기능은 아니지만 너무 메이저한 연출이라고 생각해서 시도하려 했습니다. </br>
3D 셰이더는 처음 해보지만 자료는 찾으면 많을 것 같아서 과감히 찾아봤고</br>
실제로 금방 나왔습니다.</br></br>
<img src="https://github.com/user-attachments/assets/e94a089f-260b-43b0-8a76-32666b7587ef" width="500" style="height:auto;" /></br></br>
하지만 이 원리는 버텍스 포지션에 float만큼 곱하는,</br>
센터가 정확하고 대칭인 도형에 한해 돌아갈만한 방식입니다.</br></br>
<img src="https://github.com/user-attachments/assets/45b35369-4e05-46ba-b4a7-5cfdfa5c469e" width="800" style="height:auto;" /></br></br>

multiply가 아니라 법선벡터로 add하는 방법도 있었는데, 적용해보고 알게 된 건</br>
**정육면체의 꼭지점에는 버텍스 하나가 아니라, 면이 3개니까 각 소속으로서의 3개의 버텍스가 있다**는 점이었습니다.</br>
모든 법선의 평균을 구해서 저 3개의 버텍스가 같은 방향으로 움직여주던지 해야 면들이 쪼개지지 않게 보였고</br>
실제로 3D 모델링 프로그램에서 smooth shading 옵션이 있다는 것 같습니다.</br>
블렌더로 스파이크용 원뿔을 이미 fbx로 뽑아보기도 하고 하면 될 것 같은데</br>
smooth shading으로 같은 위치 버텍스들이 같은 법선 가지게 한 다음에</br>
그쪽 방향으로 버텍스를 옮기면 메쉬입장에서는 살짝 잡아당겨지고 전체적으로 부피가 커지고</br>
그걸 뒷면 렌더하면 적당한 외곽선이 나올 거라는 예상.</br>
셰이더그래프 말고 코딩으로 직접 만들어도 구현이 된다는 것 같긴 한데</br>
코딩으로 셰이더 짜는 건 일단은 아예 모르고 과제 필수 기능과 도전 기능을 우선하기 위해</br>
일단은 위에 있는 스케일 타입 윤곽선으로 진행했습니다.</br>

## 아이템 효과 다양성
과제 수준에서는 필요 없는 규모일 수 있지만</br>
강의에서처럼 아이템 타입을 enum으로 정의하면 스타크래프트로 치면</br>
거리 내 자동 반응이나, 핵같은 지연 시전, 스톰과 플레이그의 디테일 차이 등 거의 스킬 갯수만큼의 enum 타입이 나눠질 것이고</br>
타입에 맞게 로직이 정의된 스크립트로 보내주는 조건 분기자도 있어야 하니</br>
그 switch문에 담길 10가지가 넘는 것들의 모습이 떠올랐습니다. </br>
어쩌면 그냥 그 길로 가도 가독성에 큰 문제가 없을 수 있고 스타크래프트 수준이어도 생각보다 몸집이 안 클 수도 있지만</br>
타입을 담고 로직을 따로 파는 대신 스크립터블에 애초에 로직을 담는 방법이 있지 않을까 하고 찾아보았습니다.</br>
</br>
인터페이스는 스크립터블에 담을 수가 없고</br>
직렬화된 추상 클래스를 담는다는 방법이 있어 그 방향으로 구조를 짰습니다.</br>
[아이템 데이터]라는 스크립터블의 멤버로 [스킬 로직 스크립터블]을 담는 구조가 되었습니다.</br></br>
![Unity_DPPcV7PEPu](https://github.com/user-attachments/assets/86378c81-34b3-4aa9-8c7e-714bbdfa79be)
</br></br>
**아이템 이펙트 안에는 서로 다른 타입의 스크립터블** 파일들이 만들어지겠지만(다양한 아이템 효과 로직이 담기기 때문)</br>
**상위 폴더에서는 ItemData라는 같은 타입의 스크립터블**이 이름만 다른 채로 만들어지는 그런 형태입니다.</br>
</br>
메이플스토리 버프처럼 지속시간이 얼마가 남았는지 추적하려면 모노비헤이비어로 생명주기를 알 필요가 있었고</br>
코루틴을 쓰면 실시간으로 경과 시간 계산이 안 되어</br>
기본적으로 코루틴 없이 구현되는 컨셉이지만 과제에 맞춰 타이머 종료 처리를 간단한 코루틴으로 만들어두었습니다.</br>
한편 지속시간이 필요한 아이템도 있고, 폭탄 던지기 같은 즉발 후 그냥 내려놓으면 되는 아이템도 있을텐데</br>
아이템 효과용 스크립터블을 만들었지만 지속시간을 계산하는 공공 기능이 따로 있기를 바랐습니다.</br>
그래서, 인스펙터 상에선 플레이어가 여러 개의 타이머를 다는 것처럼 되겠지만</br>
복수의 아이템 타이머 컴포넌트가 붙어 각자 필요한 걸 참조한 상태로 돌아가게 의도했습니다.</br>
적용중인 아이템 효과 리스트 정도로 프레임당 수백수천번의 컴포넌트 생성 파괴를 하지는 않을테니</br>
과감하게 아이템 타이머 컴포넌트 정도는 AddComponent로 붙이게 했습니다.</br>
다양한 아이템을 만들 시간에 다른 걸 먼저 구현해보고 싶어서 일단은 부스터와 힐 둘 뿐이지만</br>
아마 다른 효과 아이템이 많아져도 잘 작동할 것 같습니다.</br> 

## 움직이는 발판의 하강 시 문제
플랫폼은 정해진 속도로 내려오는데 플레이어는 플랫폼이 내려가는 그 찰나 속도 0에서부터 중력을 받아 내려오기 때문에</br>
덜컹거리는 문제가 있었습니다</br>
지금 플랫폼을 플랫폼의 이동을 담당하는, 본인 포지션이 움직이지는 않는 pivot 격 되는 플랫폼 핸들러가 있고</br>
실제로 움직이며 OnCollision 이벤트를 담당하기 위한 그냥 플랫폼 컴포넌트가 따로 분리되어 있는데</br>
플랫폼 컴포넌트에서는 무슨 수단을 써도 덜컹거림을 막지 못했습니다.</br>
OnTriggerExit이 항상 먼저 호출되어버렸기 때문입니다.</br>
대신 플랫폼 컴포넌트가 자의로 왕복하는 게 아니라</br>
플랫폼 핸들러에서 플랫폼을 움직여주고 있기 때문에</br>
핸들러 컴포넌트의 플랫폼 이동 메서드에서 플랫폼 컴포넌트의 메서드(올라탄 개체들의 좌표 강제)를 호출해서</br>
플랫폼이 움직이는 즉시 자식들 좌표 처리를 해주는 식으로 문제를 해결하였습니다.

## 올라탄 물체의 속도
2D 게임 기조를 머릿속에 담고 구현했고</br>
애초에 급 정속하강하는 플랫폼 자체가 현실의 물리력과 맞지 않는 부분</br>
그래서 올라탄 물체가 플랫폼의 물리 상태를 전이받을 것인지</br>
CollisionExit하면 아무 일도 없었다는 폼을 가질지</br>
이건 원하는 조작감에 따른 선택의 영역이라고 판단했습니다.</br>
소닉의 경우 경사면을 타고 올라가다가 점프를 하면 엄청 높이 올라가지만</br>
플랫폼에서 제자리 점프할 때는 플랫폼의 이동 방향으로 받는 에너지는 다 내다버리고</br>
수직 점프를 했던 걸로 기억합니다.</br>
다만 지금 소닉처럼 점프한 후에도 조금씩 움직일 에너지를 받을 수 있는 식으로 만들어두지는 않아서</br>
플랫폼 위에서 점프해서 어딘가로 가려면 갑갑함을 받는 상황입니다.</br>
플랫폼의 에너지를 받는 방법으로 OnCollisionExit 순간에 AddForce Forcemode.Accelerate를 걸어봤지만</br>
체감할 수 있는 차이가 없었습니다.</br>   
drag 때문에 공중에서 오히려 감속하고 있어서 그럴 수도 있겠다 생각합니다</br>
플레이어가 플랫폼을 참조해서 점프 로직에서 애초에 플랫폼의 속도를 합해주면 명료하겠지만</br>
점프 후에도 입력대로 움직이는 쪽으로 구현하는 게 좋다고 생각하여 아이디어만 리드미에 남겨둡니다.</br>
