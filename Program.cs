using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading; //  Thread.Sleep 사용을 위해 추가. ( 딜레이 발생 )
using System.Threading.Tasks;
/***********************************************************

                      Creator : 임성현
-------------------------------------------------------------
                     -Project : QWERTY-
-------------------------------------------------------------

 패에 들어온 카드의 효과를 사용하여 상대를 무찌르는 게임의 제작을
                      시도해보았습니다.
  주석으로 카드의 효과를 설명하거나 하는 등의 부분은 만약
        실제 게임을 제작한다면 넣고 싶은 효과 등으로
            해당 콘솔에는 구현되어있지 않습니다.
 함수 등에는 주석으로, 게임 내부 구현을 위한 코드는 리전으로 묶어
        확인하고 싶은 코드를 펼쳐 확인할 수 있게 하였습니다.
-------------------------------------------------------------

 스토리 짜다 멘탈 갈려서 인트로 빼고 그냥 전부 스킵 처리했습니다
    스토리 구현을 확인하려면 아래 설명의 인트로, 조우 함수를
           게임 쪽에서 찾아 주석을 해제해 주세요.

-------------------------------------------------------------
                     스토리 미구현 목록
-------------------------------------------------------------

                     2번째 적과의 조우
                     3번째 적과의 조우
                     마지막 적과의 조우
               이동 과정에서 겪는 상실감 이벤트

-------------------------------------------------------------

************************************************************/




namespace ConsoleProject
{
    internal class Program
    {
        //조작 방법을 알려주는 튜토리얼입니다.
        static void tutorial()
        {
            Console.WriteLine("적과 관련된 인터페이스는 붉은색, 캐릭터와 관련된 인터페이스는 초록색으로 나옵니다.");
            Console.WriteLine("Q,W,E,R,T,Y 6개의 키를 입력하여 각 키에 할당된 카드를 사용할 수 있습니다.\n");
            Console.WriteLine("카드를 사용한 경우 해당 카드의 자리에는 파워 0의 카드, 빈 카드가 생기며");
            Console.WriteLine("빈 카드를 사용할 경우 카드를 사용하지 않은 것으로 취급, 자신의 턴을 건너뜁니다.\n");
            Console.WriteLine("빈 카드를 제외한 모든 카드는 행동 포인트를 소모합니다.");
            Console.WriteLine("행동 포인트가 0이 될 경우 포인트를 10으로 보충하는 대신, 해당 턴 카드를 사용할 수 없습니다.\n");
            Console.WriteLine("패는 6장을 전부 사용하거나 12번 카드 사용 시에 다시 보충됩니다.");
            Console.WriteLine("14번 카드 \"조커\"는 12번 카드를 사용한 셔플 시에만 등장할 수 있습니다.\n");
            Console.WriteLine("적의 행동 위력보다 낮은 파괴력을 가지는 카드를 사용했을 경우, 피해를 입힐 수 없습니다.");
            Console.WriteLine("또한, 공격을 제외한 적의 모든 행동이 성공적으로 이루어지니, 최대한 행동 위력 이상의 파괴력을 가진 카드를 사용합시다.\n");
            Console.WriteLine("적의 체력이 0이 되면 다음 스테이지로 이동하며, 총 4개의 적을 상대해야 합니다.");
            Console.WriteLine("4번째 적을 쓰러트리면 클리어, 플레이어의 체력이 0이 되면 패배합니다.\n");
            Thread.Sleep(3000);
            Console.WriteLine("아무 키나 눌러 진행합니다.");
            ConsoleKeyInfo skip;
            skip = Console.ReadKey(true);
            switch (skip.Key)
            {
                default:
                    Console.Clear();
                    break;
            }

        }
        #region 스토리(비활성화)

        //게임의 인트로 입니다.
        static void phase1()
        {
            //안내용 창. S키를 입력받아 인트로 스킵 / 다른 키를 눌러 진행하기.
            Console.WriteLine("S키를 누르면 인트로를 스킵하고 바로 게임으로 넘어갑니다.");
            Console.WriteLine("스토리를 보려면 S 이외의 키를 눌러주세요!");
            Console.WriteLine("\n절대 키를 0.4초 이상 누르지 마십시오.");
            ConsoleKeyInfo skip;
            //읽을 시간 제공
            Thread.Sleep(3000);
            //안내용 문자 삭제.
            Console.Clear();
            //아래는 인트로 스토리.
            string[] intro = new string[15];
            intro[0] = "세상을 탐험하며 발굴하는 것을 일삼는 이들을 탐험가라고 해.";
            intro[1] = "그리고 그런 탐험가들은 대체로 어떤 곳을 조사해달라는 의뢰를 통해 생계를 해결하지.";
            intro[2] = "나 또한 탐험가로서 의뢰를 받고, 사람이 실종되었다는 동굴을 조사하고 있어.";
            intro[3] = "이번 의뢰를 무사히 마치면, 동생을 치료할 약을 구할 수 있겠지...";
            intro[4] = "동굴을 탐험하는 도중 정체불명의 괴물에게 공격받았고.";
            intro[5] = "눈앞이 흐릿해져 갈 즈음, 머리속에 정체 모를 목소리가 울리기 시작했어.";
            intro[6] = "\"이런 곳에서 포기할 거니?\"";
            intro[7] = "...아니";
            intro[8] = "여기서 포기할 수 있을 리가 없잖아...";
            intro[9] = "\"그럴 줄 알았어. 인간들은 정말 알기 쉽다니까?\"";
            intro[10] = "\"좋아. 기회를 줄게. 너가 운명을 거스를 수 있는 인물이라는 것을, 스스로 증명해 보도록 해.\"";
            intro[11] = "그 직후 정신을 잃었고, 눈을 떴을 땐 어디인지 감도 잡을 수 없는 공간 속에 덩그러니 놓여있었어.";
            intro[12] = "절망적이게도 지금 갖고 있는 건 내 유일한 무기, 여섯 장을 다 쓰면 다시 리필되는 카드 보관함 하나뿐.";
            intro[13] = "그래도, 두고 보라고. 이 앞에서 어떤 일이 일어나더라도,";
            intro[14] = "반드시 살아서 돌아가고 말 테니.";

            //S키가 눌려져 있다면 바로 스킵하고 로딩으로, 그 외에는 스토리 진행.
            for(int i = 0; i < intro.Length; i++)
            {
                skip = Console.ReadKey(true);
                switch (skip.Key)
                {
                    case ConsoleKey.S:
                        i = 16;
                        break;
                    default:
                        if(i == 4 || i == 11)
                        {
                            Console.Clear();
                        }
                        Console.WriteLine(intro[i]);
                        if(i == 5||i==8)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                        }
                        else if (i==6||i==10)
                        {
                            Console.ResetColor();
                        }
                            Thread.Sleep(1500);
                        break;

                }

            }
            //스토리를 봤을 경우를 대비해 삭제
            Console.Clear();

            //로딩 연출(사실 넣어보고 싶었습니다)
            Console.WriteLine("Project : QWERTY");
            Console.Write("Loading");
            Thread.Sleep(1000);
            Console.Write(".");
            Thread.Sleep(1000);
            Console.Write(".");
            Thread.Sleep(1000);
            Console.Write(".");
            Thread.Sleep(1000);
            Console.WriteLine("\n로딩 완료. 첫 스테이지를 불러옵니다.");
            Thread.Sleep(3000);
            Console.Clear();
        }

        //첫 번째 적을 조우할 때 나오는 "조우" 이벤트입니다.
        static void Enter()
        {

            //게임 인트로 부분과 같이 스킵 기능 제공
            ConsoleKeyInfo skip;
            string[] Enter = new string[3];
            Enter[0] = "이봐! 정신 차려라! 이런 곳에서 뭘 하고 있는 거냐!";
            Enter[1] = "비켜. 난 해야 할 일이 있다고...";
            Enter[2] = "제정신인 거냐! 어서 물러나라! 여긴 위험하단 말이다!";
            for (int i = 0; i < Enter.Length; i++)
            {
                skip = Console.ReadKey(true);
                switch (skip.Key)
                {
                    //배열이 3개의 공간을 가지므로 가진 공간의 최대 수를 넘겨 for문을 탈출
                    case ConsoleKey.S:
                        i = 3;
                        break;
                    default:
                        //배열 0과 2에 있는 대사는 적이 하는 대사이므로 색상을 변경
                        if (i == 0 || i == 2)
                        {
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        }
                        Console.WriteLine(Enter[i]);
                        Console.ResetColor();
                        Thread.Sleep(1500);
                        break;

                }
            }

            //전부 출력 후 콘솔창 내용 삭제
            Console.Clear();
        }
        #endregion
        #region 엔딩

        //체력이 0이 되어 패배한 경우 나오는 배드 엔딩입니다.
        static void FailEnd()
        {
            Thread.Sleep(1500);
            //머리속에 울리던 그 목소리의 대사.
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\"살아서 나가겠다던 네 각오는 고작 이 정도였던 거야?\"");
            Thread.Sleep(1500);
            Console.WriteLine("\"실망스럽네.\"");
            Thread.Sleep(1500);
            Console.ResetColor();
            Console.Clear();
            Thread.Sleep(1500);
            //나레이션
            Console.WriteLine("당신의 존재가 서서히 먼지가 되어 사라지는 것이 느껴집니다.");
            Thread.Sleep(1500);
            Console.WriteLine("운명을 거스르기 위한 스스로의 증명에 실패했습니다.");
            Thread.Sleep(1500);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Dead End");

        }

        //4번째 적 "보스"를 쓰러트렸을 때 나오는 원래의 엔딩입니다.
        static void RealEnd()
        {
            //대사 모음
            string[] epiloge = new string[16];
            epiloge[0] = "앞을 가로막는 적을 모두 무찌르고, 난 앞으로 계속해서 나아갔다.";
            epiloge[1] = "그리고 마침내, 하나의 빛줄기를 발견했고,";
            epiloge[2] = "빠져나갈 수 있다는 생각에 계속해서 달려나가기 시작했다.";
            epiloge[3] = "죽을 뻔했던 상황에서 기적처럼 돌아온 나의 이야기는";
            epiloge[4] = "많은 탐험가들 사이에서 퍼져나갔다.";
            epiloge[5] = "의뢰를 실패할 거라고 생각했던 고용주는 내 모습을 보자 화들짝 놀라 도망치기 시작했고,";
            epiloge[6] = "죽기 직전까지 갔던 나는 그를 붙잡아 확실하게 따지기 시작했다.";
            epiloge[7] = "의뢰 비용과 위험 수당까지 추가로 뜯어내서, 약을 구하는 데에는 성공했다.";
            epiloge[8] = "이제 동생의 병을 치료할 수 있을 거야.";
            epiloge[9] = "그런데... 왜 자꾸 뭔가를 잊어버린 느낌이 드는 거지?";
            epiloge[10] = "Normal End = 작은 희망";
            epiloge[11] = "「자네, 그 소식 들었나? 얼마 전에 유명세를 얻은 그 탐험가 말이야.」";
            epiloge[12] = "「그 괴짜 녀석 말인가. 그래, 탐험 동기가 동생의 병을 치료할 돈을 위해서라고 했던가?」";
            epiloge[13] = "「그래. 참으로 이상한 일이지. 그도 그럴 것이...」";
            epiloge[14] = "「그 녀석, 동생이 없지 않은가.」";
            epiloge[15] = "Normal End = 헛된 희망";

            //S키가 눌려져 있다면 바로 스킵하고 로딩으로, 그 외에는 스토리 진행.
            for (int i = 0; i < epiloge.Length; i++)
            {

                //배열의 5번 칸과 10번 칸의 내용 출력 차례에 남아있던 이야기 전부 삭제
                if (i == 5 || i == 10)
                {
                    Console.Clear();
                }

                //배열의 15번째 칸은 기존에 작성한 10번째 칸 코드를 그대로 덮어씌워야 하며, 색을 회색으로 맞추어야 함.
                else if (i == 15)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.SetCursorPosition(0, 0);
                }
                Console.WriteLine(epiloge[i]);
                Console.ResetColor();
                Thread.Sleep(1500);
            }
            Console.SetCursorPosition(6, 0);
        }
        #endregion
        #region 함수

        //패에 잡힌 카드를 전부 사용했을 경우, 패를 리필하기 위해 사용합니다. 고정적으로 6을 기입합니다.
        static int[] Shuffle(int deck)
        {
            //패에 잡게 될 카드의 수를 입력받아, 해당 수만큼의 공간을 갖는 배열을 생성합니다.
            //고정적으로 6을 기입하므로, 선택할 수 있는 카드의 수는 총 6개가 됩니다.
            int[] hand = new int[deck];

            //랜덤 변수를 선언합니다. 지정된 범위까지의 숫자 중 랜덤으로 카드를 잡게 해 줍니다.
            Random shuff = new Random();

            //배열 내의 6개의 공간에 1부터 13까지의 랜덤한 수를 기입합니다.
            //조커인 14는 12번 카드 "퀸"의 효과로만 획득 가능하므로 획득할 수 없도록 조정하였습니다.
            for(int i = 0; i<hand.Length;i++)
            {
                hand[i] = shuff.Next(1, 14);
            }

            //랜덤한 수를 갖는 6개의 패 배열을 내보냅니다.
            return hand;
        }

        //아르카나 퀸의 효과 셔플&힐로 인해 패를 리필할 경우 사용합니다. 고정적으로 6을 기입합니다.
        //위 Shuffle에서 14번 카드 "조커"를 잡을 수 있게 되었다는 것 외에는 위 Shuffle과 동일하므로 설명은 생략합니다.
        static int[] QueenShuffle(int deck)
        {
            int[] hand = new int[deck];
            Random shuff = new Random();
            for (int i = 0; i < hand.Length; i++)
            {
                hand[i] = shuff.Next(1, 15);
            }
            return hand;
        }

        //패에 잡힌 카드를 사용하는 경우, 공격력만을 계산하기 위한 함수입니다.
        static double Attack(int power, double mobHP)
        {
            //실제 가하게 될 데미지의 배율을 계산합니다.
            double damage = 1;
            switch(power)
            {

                //원래 코드를 짤 때는 0번이 적힌 카드는 다시 돌려보내는 식으로 작성했었지만,
                //수정 과정에서 0번을 사용 가능하게 될 수도 있어,
                //그런 상황에는 0번 사용 시 행동하지 않고 턴을 진행하도록 코드를 작성했습니다.
                case 0:
                    Console.WriteLine("\"파워... 잠깐, 효과가 없는 카드잖아!\"");
                    Console.WriteLine("빈 카드를 골라 데미지를 가할 수 없어 턴을 건너뜁니다.");
                    damage = 0;
                    break;
                //1번 카드는 데미지를 2배로 하여 상대를 공격하는 카드입니다.
                case 1:
                    Console.WriteLine("\"파워 1 - 투창 카드 사용!\""); // 데미지 배율 2
                    damage *= 2;
                    break;
                case 2:
                    Console.WriteLine("\"파워 2 - 방어 태세 카드 사용!\""); // 받는 데미지 감소 : 함수 밖에서 계산
                    break;
                case 3:
                    Console.WriteLine("\"파워 3 - 맹세 카드 사용!\""); // 데미지 경감 : 함수 밖에서 계산
                    //데미지 배율 1.5배
                    damage *= 1.5;
                    break;
                case 4:
                    Console.WriteLine("\"파워 4 - 혈투 카드 사용!\""); // 체력 비례 대미지
                    damage = (double)mobHP * 0.15;
                    break;
                case 5:
                    Console.WriteLine("\"파워 5 - 버서크 카드 사용!\""); // 버서크 버프 액티브
                    break;
                case 6:
                    Console.WriteLine("\"파워 6 - 연속 사격 사용!\""); // 데미지 배율 2배, 2회 -> 4배
                    damage *= 4;
                    break;
                case 7:
                    Console.WriteLine("\"파워 7 - 더블업 카드 사용!\""); // 더블업 버프 액티브
                    break;
                case 8:
                    Console.WriteLine("\"파워 8 - 리커버 카드 사용!\""); // 리커버 버프 액티브
                    break;
                case 9:
                    Console.WriteLine("\"파워 9 - 리플렉트 카드 사용!\""); // 리플렉트 트리거 액티브
                    break;
                case 10:
                    Console.WriteLine("\"파워 10 - 얼티밋 스트라이크 카드 사용!\""); // 데미지 배율 5.5배
                    damage *= 5.5;
                    break;
                case 11:
                    Console.WriteLine("\"파워 11 - 아르카나 잭 - 프리페어 카드 사용!\""); // 프리페어 버프 액티브
                    break;
                case 12:
                    Console.WriteLine("\"파워 12 - 아르카나 퀸 - 셔플&힐 카드 사용!\""); // 퀸 전용 셔플 발동, 함수 밖에서 회복
                    break;
                case 13:
                    Console.WriteLine("\"파워 13 - 아르카나 킹 - <로얄> 스트레이트 플러시 카드 사용!\""); // 데미지 배율 10배
                    damage *= 10;
                    break;
                case 14:
                    Console.WriteLine("\"파워 14 - 조커 - 리퍼 사이드 카드 사용!\""); // 데미지 배율 7배, 2회 -> 14배
                    damage *= 14;
                    break;
            }
            //데미지의 배율을 내보냅니다.
            return damage;

        }
        #endregion
        #region 열거형

        // 플레이어의 패에 들어올 수 있는 카드가 담는 효과의 모음입니다.
        enum Hand
        {
            빈칸, 단타, 방어, 맹세, 혈투, 폭주, 연타, 강화, 회복, 반탄, 강타, 준비, 셔플, 필살, 조커
        }

        //상대가 행할 행동의 모음입니다.
        enum Act
        {
            공격, 방어, 회피, 연타, 폭주, 강타
        }
        #endregion
        #region 구조체
        //패에 들어오는 카드의 자료형입니다.
        struct MyHand
        {
            //카드의 파괴력. 숫자가 높아질수록 강하며, 상대보다 낮은 숫자일 경우 공격이 실패합니다.
            public int Power;

            //카드의 이름
            public string Name;

            //해당 카드가 소모하게 될 행동 포인트입니다. 파괴력 수치가 높아질수록 소모하는 행동 포인트 또한 높아집니다.
            public int ReqAP;

            //해당 카드가 가지게 될 효과입니다. 모든 카드는 기본적으로 공격력 수치만큼의 공격을 가합니다.
            public Hand Effect;
        }

        //상대하게 될 적의 정보를 담는 자료형입니다.
        struct Enemy
        {
            //공격력을 결정합니다. 공격 시 해당 수치에 지정된 배율만큼의 데미지를 플레이어에게 가합니다.
            public int AtkPoint;

            //체력을 결정합니다. 해당 수치가 0이 되었을 경우 처치 판정으로, 다음 적으로 넘어갑니다.
            public double HealthPoint;

            //카드와 위력을 비교할 전용 수치입니다. 해당 수치가 플레이어보다 낮을 경우 공격 이외의 패턴은 파훼됩니다.
            public int Power;

            //해당 적이 가지게 될 행동 양식입니다. 보스를 기준으로 맞추고, 메인 함수에서 하위 단계 적들은 사용하지 못하는 패턴을 지정하여 억제합니다.
            public Act TurnAct;

            //방어 성공 시 피해량에 적용하는 비율입니다.
            public double DmgReduction;
        }

        //플레이어의 기본 정보입니다. 카드의 효과는 해당 정보를 참고하여 특정 배율을 적용합니다.
        struct Player
        {
            //공격력을 결정합니다. 모든 카드는 기본적으로 해당 공격력만큼의 공격을 가하며, 배율을 적용하는 카드는 이 정보에 영향을 받습니다.
            public int AtkPoint;

            //방어력을 결정합니다. 상대가 보여준 수치보다 내 카드의 공격력이 높을 때 상대가 공격한 경우, 해당 수치만큼 데미지를 경감합니다.
            public int DefPoint;

            //행동 카운트입니다. 해당 수치가 0이 되거나 그 아래로 떨어지는 경우, 전부 채우기 위해 한 턴 동안 행동할 수 없습니다.
            public int ActPoint;

            //체력을 결정합니다. 해당 수치가 0이 될 경우 패배하여 게임이 종료됩니다.
            public double HealthPoint;
        }
        #endregion
        static void Main(string[] args)
        {
            #region 설정
            #region 플레이어 설정

            //리그 오브 레전드의 트위스티드 페이트에서 카드를 사용하는 캐릭터의 컨셉을 인용하였습니다.
            Player TwistedFate;

            //기본적으로 주어지는 행동 포인트는 10입니다.
            TwistedFate.ActPoint = 10;

            //기본적인 공격력은 100입니다. 모든 카드 사용 시 해당 공격력을 기준으로, 배율을 적용하는 카드는 해당 배율을 적용하여 공격합니다.
            TwistedFate.AtkPoint = 100;

            //기본적인 방어력은 30입니다. 상대의 수치보다 파괴력이 높을 경우, 상대의 공격을 해당 방어력만큼 경감하여 받게 됩니다.
            TwistedFate.DefPoint = 10;

            //기본적인 체력은 2500입니다. 상대의 공격을 받을 때 피해를 받은 만큼 감소하여, 0이 되면 사망하여 패배 처리됩니다.
            TwistedFate.HealthPoint = 2500;

            #endregion
            #region 카드의 효과

            //들어올 수 있는 카드의 종류는 총 14장입니다. 사용하여 아직 들어오지 않은 카드를 포함하기 위해 한 개를 더 추가합니다.
            MyHand[] holdable = new MyHand[15];

            //0번 카드는 빈 카드입니다.
            holdable[0].Power = 0;
            holdable[0].Name = "빈 카드";
            holdable[0].ReqAP = 0;
            holdable[0].Effect = Hand.빈칸;

            //1번 카드는 카드를 적에게 던져 날카로운 일격을 가합니다. ( 배율 2 )
            holdable[1].Power = 1;
            holdable[1].Name = "투창";
            holdable[1].ReqAP = 1;
            holdable[1].Effect = Hand.단타;

            //2번 카드는 방어막을 전개하여 상대의 공격을 일부 경감합니다.
            holdable[2].Power = 2;
            holdable[2].Name = "방어 태세";
            holdable[2].ReqAP = 1;
            holdable[2].Effect = Hand.방어;

            //3번 카드는 각오를 다져 해당 턴 받는 피해를 줄이며 가하는 공격력을 증가시킵니다. ( 20 감소, 배율 1.5 )
            holdable[3].Power = 3;
            holdable[3].Name = "맹세";
            holdable[3].ReqAP = 1;
            holdable[3].Effect = Hand.맹세;

            //4번 카드는 죽을 각오로 상대를 공격하여 최대 생명력의 일정 비율만큼 피해를 입힙니다.
            holdable[4].Power = 4;
            holdable[4].Name = "혈투";
            holdable[4].ReqAP = 1;
            holdable[4].Effect = Hand.혈투;

            //5번 카드는 자신을 보조하는 마법으로, 공격에 2.5배의 배율을 추가합니다.
            holdable[5].Power = 5;
            holdable[5].Name = "버서크";
            holdable[5].ReqAP = 2;
            holdable[5].Effect = Hand.폭주;

            //6번 카드는 카드를 동시에 두 개 던져, 피해를 두 번 입힙니다.
            holdable[6].Power = 6;
            holdable[6].Name = "연속 사격";
            holdable[6].ReqAP = 2;
            holdable[6].Effect = Hand.연타;

            //7번 카드는 자신을 보조하는 스택형 마법으로, 다음 턴부터 피해량을 두 배로 적용합니다.
            holdable[7].Power = 7;
            holdable[7].Name = "더블업";
            holdable[7].ReqAP = 2;
            holdable[7].Effect = Hand.강화;

            //8번 카드는 자신을 보조하는 마법으로, 소모되어있던 행동 카운트를 3 회복해줍니다.
            holdable[8].Power = 8;
            holdable[8].Name = "리커버";
            holdable[8].ReqAP = 2;
            holdable[8].Effect = Hand.회복;

            //9번 카드는 자신을 보조하는 마법으로, 상대가 공격을 행할 때 사용할 경우 그 피해를 없애며,
            //해당 공격으로 입었어야 할 피해를 자신의 공격에 합산하여 상대에게 공격을 가합니다.
            holdable[9].Power = 9;
            holdable[9].Name = "리플렉트";
            holdable[9].ReqAP = 2;
            holdable[9].Effect = Hand.반탄;
            
            //10번 카드는 카드를 적에게 던져, 적에게 닿는 순간 마법으로 거대화시켜 큰 피해를 입힙니다.
            holdable[10].Power = 10;
            holdable[10].Name = "얼티밋 스트라이크";
            holdable[10].ReqAP = 3;
            holdable[10].Effect = Hand.강타;
            
            //11번 카드는 잭. 다음 행동으로 소모될 행동 카운트의 소모량을 없애줍니다.
            holdable[11].Power = 11;
            holdable[11].Name = "프리페어";
            holdable[11].ReqAP = 3;
            holdable[11].Effect = Hand.준비;
            
            //12번 카드는 퀸. 가지고 있는 카드와 사용한 카드를 새로운 카드로 교체하며, 약간의 체력과 행동 카운트를 회복합니다.
            //낮은 확률로 조커를 획득합니다.
            holdable[12].Power = 12;
            holdable[12].Name = "셔플 & 힐";
            holdable[12].ReqAP = 4;
            holdable[12].Effect = Hand.셔플;

            //13번 카드는 킹. 5개의 카드를 적에게 던지며, 카드들은 적에게 닿는 순간 폭발을 일으킵니다.
            holdable[13].Power = 13;
            holdable[13].Name = "스트레이트 플러시";
            holdable[13].ReqAP = 5;
            holdable[13].Effect = Hand.필살;

            //14번째 카드는 조커. 통상적인 방법으로는 획득할 수 없습니다.
            //조커를 소환하여 낫으로 상대를 2번 베고, 해당 공격으로 준 피해만큼 체력을 회복합니다.
            holdable[14].Power = 14;
            holdable[14].Name = "조커 - 리퍼 사이드";
            holdable[14].ReqAP = 7;
            holdable[14].Effect = Hand.조커;
            #endregion
            #region 적의 기본 정보

            //등장하는 적은 총 4명입니다.
            Enemy[] enemies = new Enemy[4];

            //첫 번째 적입니다.
            enemies[0].AtkPoint = 100;
            enemies[0].TurnAct = Act.공격;
            enemies[0].HealthPoint = 700;
            enemies[0].DmgReduction = 0.5;

            //두 번째 적입니다.
            enemies[1].AtkPoint = 180;
            enemies[1].TurnAct = Act.공격;
            enemies[1].HealthPoint = 2000;
            enemies[1].DmgReduction = 0.4;

            //세 번째 적입니다.
            enemies[2].AtkPoint = 250;
            enemies[2].TurnAct = Act.공격;
            enemies[2].HealthPoint = 4500;
            enemies[2].DmgReduction = 0.25;

            //마지막 적입니다. 해당 적을 물리치면 승리 판정으로 게임이 종료됩니다.
            enemies[3].AtkPoint = 400;
            enemies[3].TurnAct = Act.공격;
            enemies[3].HealthPoint = 9000;
            enemies[3].DmgReduction = 0.1;
            #endregion
            #region 게임 내부 설정
            //공격력을 계산하기 위한 파괴력 수치입니다.
            int power = 0;
            //패에 잡은 카드의 파괴력 수치를 임의의 수로 만들어줄 랜덤 변수입니다.
            Random Power = new Random();
            //유저가 적에게 가하게 될 피해의 수치입니다.
            double pDamage = 0;
            //현재 상대하고 있는 적의 순서입니다.
            int phase = 0;
            //게임이 유지되고 있는지 확인합니다.
            bool isGameOn = true;
            //초기 플레이어의 패를 설정, 6개의 패를 랜덤으로 설정합니다.
            int[] deck;
            deck = Shuffle(6);
            //몬스터의 행동을 설정할 변수를 설정합니다.
            int pattern = 0;
            //버서크로 인해 증가하게 될 피해량 변경 적용을 위한 변수를 설정합니다.
            double dmgMultiplyer = 1;
            #endregion
            #region 버프 정보

            //2번 효과 방어 태세의 활성화 여부를 판단합니다.
            bool isGuardActive = false;

            //3번 효과 맹세의 활성화 여부를 판단합니다.
            bool isDecisionActive = false;

            //5번 효과 버서크의 활성화 여부를 판단합니다.
            bool isBerserkActive = false;

            //7번 효과 더블업의 활성화 여부를 판단합니다.
            bool isDoubleUpActive = false;

            //8번 효과 리커버의 활성화 여부를 판단합니다.
            bool isRecoverActive = false;

            //9번 효과 리플렉트의 활성화 여부를 판단합니다.
            bool isReflectActive = false;

            //11번 효과 프리페어의 활성화 여부를 판단합니다.
            bool isPrepareActive = false;

            //보스의 버서크 상태의 활성화 여부를 판단합니다.
            bool isBossBerserkActive = false;

            //보스의 버서크 상태의 턴 경과 수를 기록합니다.
            int berserkTurn = 0;
            #endregion
            #endregion
            #region 게임

            //게임의 인트로 phase1을 재생합니다.
            //phase1();

            //첫 번째 적과 조우하여 발생하는 이벤트 Enter을 재생합니다.
            //Console.WriteLine("\"조우\" 이벤트를 스킵하려면 S키를 눌러주세요!");
            //Enter();

            //본 게임에 들어가기 앞서 튜토리얼을 위한 설명을 띄웁니다.
            tutorial();

            //처음 시작 시 적의 행동 위력과 행동 패턴을 실행시킵니다.
            enemies[phase].Power = Power.Next(0, 9);
            enemies[phase].TurnAct = (Act)pattern;
            //게임이 실행되고 있는 동안 다음 작업을 실행합니다.
            while (isGameOn)
            {
                #region 페이즈별 캐릭터 대사
                //페이즈별 캐릭터가 느끼는 감정을 표현
                switch(phase)
                {
                    case 0:
                        Console.WriteLine("나아가야 해.");
                        Console.WriteLine("");
                        break;
                    case 1:
                        Console.WriteLine("뭔가를 잊어버린 느낌인데...");
                        Console.WriteLine("");
                        break;
                    case 2:
                        Console.WriteLine("...내가 왜 여기 있었더라?");
                        Console.WriteLine("");
                        break;
                    case 3:
                        Console.WriteLine("...공허해");
                        Console.WriteLine("");
                        break;
                }
                #endregion
                #region 플레이어의 행동 요구

                //적과 관련된 정보 - 적의 행동과 위력, 체력 제공
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"행동 {enemies[phase].TurnAct} \t\t위력 {enemies[phase].Power} \t\t체력 {enemies[phase].HealthPoint} \t\t공격력 {enemies[phase].AtkPoint}");
                Console.WriteLine("");

                //플레이어에 관련된 정보 - 플레이어의 체력과 행동 포인트, 소지 카드 제공
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\t체력 {TwistedFate.HealthPoint} \t\t공격력 {TwistedFate.AtkPoint} \t\t행동P {TwistedFate.ActPoint}");
                Console.WriteLine("");

                //"사용할 카드" 값 미리 초기화
                int usedOne = 0;
                
                // 랜덤 변수로 handCard(손패) 지정
                ConsoleKeyInfo handCard;

                //플레이어의 행동 포인트가 0이 되었을 경우, 해당 턴 동안 휴식을 취하며 행동 포인트를 10 보충
                Console.ResetColor();
                if(TwistedFate.ActPoint <= 0)
                {
                    Console.WriteLine("행동 포인트가 0이 되어, 행동 포인트를 보충합니다.");
                    TwistedFate.ActPoint = 10;
                    Thread.Sleep(2000);
                    Console.Clear();
                    continue;
                }
                else
                {
                    //(수정 과정에서 가독성 문제로 하나하나 끊어서 입력하였습니다)
                    Console.WriteLine("공격에 사용할 카드를 선택하여 주십시오.");
                    Console.WriteLine($"카드 Q:{holdable[deck[0]].Effect}, 행동 포인트:{holdable[deck[0]].ReqAP}");
                    Console.WriteLine($"카드 W:{holdable[deck[1]].Effect}, 행동 포인트:{holdable[deck[1]].ReqAP}");
                    Console.WriteLine($"카드 E:{holdable[deck[2]].Effect}, 행동 포인트:{holdable[deck[2]].ReqAP}");
                    Console.WriteLine($"카드 R:{holdable[deck[3]].Effect}, 행동 포인트:{holdable[deck[3]].ReqAP}");
                    Console.WriteLine($"카드 T:{holdable[deck[4]].Effect}, 행동 포인트:{holdable[deck[4]].ReqAP}");
                    Console.WriteLine($"카드 Y:{holdable[deck[5]].Effect}, 행동 포인트:{holdable[deck[5]].ReqAP}");

                    //사용할 카드 입력받기
                    handCard = Console.ReadKey(true);

                    //오류 방지 1 - QWERTY 이외의 다른 키를 입력하였을 경우, 딜레이 없이 즉시 다시 요구
                    while(handCard.Key != ConsoleKey.Q && handCard.Key != ConsoleKey.W && handCard.Key != ConsoleKey.E && handCard.Key != ConsoleKey.R && handCard.Key != ConsoleKey.T && handCard.Key != ConsoleKey.Y )
                    {
                        Console.Clear();
                        Console.WriteLine("잘못된 입력을 가하였습니다. 지정된 카드를 사용하여 주십시오.");
                        Console.WriteLine("");
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine($"행동 {enemies[phase].TurnAct} \t\t위력 {enemies[phase].Power} \t\t체력 {enemies[phase].HealthPoint} \t\t공격력 {enemies[phase].AtkPoint}");
                        Console.WriteLine("");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\t체력 {TwistedFate.HealthPoint} \t\t공격력 {TwistedFate.AtkPoint} \t\t행동P {TwistedFate.ActPoint}");
                        Console.WriteLine("");
                        Console.ResetColor();
                        Console.WriteLine($"카드 Q:{holdable[deck[0]].Effect}, 행동 포인트:{holdable[deck[0]].ReqAP}");
                        Console.WriteLine($"카드 W:{holdable[deck[1]].Effect}, 행동 포인트:{holdable[deck[1]].ReqAP}");
                        Console.WriteLine($"카드 E:{holdable[deck[2]].Effect}, 행동 포인트:{holdable[deck[2]].ReqAP}");
                        Console.WriteLine($"카드 R:{holdable[deck[3]].Effect}, 행동 포인트:{holdable[deck[3]].ReqAP}");
                        Console.WriteLine($"카드 T:{holdable[deck[4]].Effect}, 행동 포인트:{holdable[deck[4]].ReqAP}");
                        Console.WriteLine($"카드 Y:{holdable[deck[5]].Effect}, 행동 포인트:{holdable[deck[5]].ReqAP}");
                        handCard = Console.ReadKey(true);
                    }
                    //입력받은 카드 사용을 위한 위치 파악
                    switch (handCard.Key)
                    {
                        case ConsoleKey.Q:
                            usedOne = 0;
                            break;
                        case ConsoleKey.W:
                            usedOne = 1;
                            break;
                        case ConsoleKey.E:
                            usedOne = 2;
                            break;
                        case ConsoleKey.R:
                            usedOne = 3;
                            break;
                        case ConsoleKey.T:
                            usedOne = 4;
                            break;
                        case ConsoleKey.Y:
                            usedOne = 5;
                            break;
                    }
                }

                //오류 방지 2 - 파워 0인 카드를 사용했을 경우 사용을 막고 방금 전 상황으로 되돌아감.
                if(deck[usedOne] == 0)
                {
                    Console.Clear();
                    Console.WriteLine("불명 카드는 사용할 수 없습니다. 다른 효과를 가진 카드를 사용하여 주십시오.");
                    Console.WriteLine("잠시 뒤 되돌아갑니다.");
                    Thread.Sleep(2000);
                    Console.Clear();
                    continue;
                }
                #endregion
                #region 카드 입력 후 남아있던 1회성 버프 제거

                //2번 효과는 방어력을 10 올려주는 효과가 남아있으므로 해당 효과를 제거합니다.
                if(isGuardActive == true)
                {
                    isGuardActive = false;
                    TwistedFate.DefPoint = TwistedFate.DefPoint - 10;
                }
                
                //3번 효과는 방어력을 20 올려주는 효과가 남아있으므로 해당 효과를 제거합니다.
                if(isDecisionActive == true)
                {
                    isDecisionActive = false;
                    TwistedFate.DefPoint = TwistedFate.DefPoint - 20;
                }

                //5번 효과는 공격력을 2.5배로 올리는 효과를 이미 적용했으므로 false로 변경합니다.
                if(isBerserkActive == true)
                {
                    isBerserkActive = false;
                }

                //7번 효과는 다음 턴에 영구적으로 공격력을 2배로 하는 효과이므로 피해량 배율을 2배로 변경합니다.
                if(isDoubleUpActive == true)
                {
                    isDoubleUpActive = false;
                    dmgMultiplyer *= 2;
                }

                //8번 효과는 행동 카운트를 3 회복하는 효과를 이미 사용하였으므로 false로 변경합니다.
                if(isRecoverActive == true)
                {
                    isRecoverActive = false;
                }

                //9번 효과는 사용 즉시 적용하여 체크하였으므로 false로 변경합니다.
                if (isReflectActive == true)
                {
                    isReflectActive = false;
                }

                //11번 효과는 묶어서 하지 않고 행동 포인트 소모 시에 따로 체크합니다.

                //보스의 버서크 효과는 4턴 동안 유지되므로 해당 턴의 확인을 진행, 4턴째인 경우 false로 전환합니다.
                if(isBossBerserkActive == true)
                {
                    berserkTurn++;
                    if(berserkTurn == 5)
                    {
                        isBossBerserkActive = false;
                        enemies[phase].AtkPoint /= 2;
                        berserkTurn = 0;
                    }
                }
                #endregion
                #region 패 선택 이후 공격 판정
                Console.Clear();

                //플레이어가 사용을 선택한 패 1장의 파괴력을 저장
                power = deck[usedOne];

                //플레이어가 사용을 선택한 패 1장의 사용 조건만큼 행동 포인트 감소
                TwistedFate.ActPoint = TwistedFate.ActPoint - holdable[power].ReqAP;

                //11번 효과는 여기서 전환. 소모한 만큼 회복하여 포인트 사용을 무마시킵니다.
                if(isPrepareActive == true)
                {
                    TwistedFate.ActPoint = TwistedFate.ActPoint + holdable[power].ReqAP;
                }

                //플레이어가 사용을 선택한 패 1장을 0으로 변환
                deck[usedOne] = 0;

                //선택한 패로 인해 적에게 입힐 데미지에 배율을 적용하는 계산하는 함수 실행
                pDamage = Attack(power, enemies[phase].HealthPoint) * TwistedFate.AtkPoint;
                Thread.Sleep(1500);

                //사용한 패의 효과가 공격력 증가 이외의 특이한 효과를 가진 경우 해당 효과의 트리거 작동
                switch (power)
                {

                    //2번 카드 가드의 효과 발동 On, 방어력을 10 증가시킵니다.
                    case 2:
                        isGuardActive = true;
                        TwistedFate.DefPoint = TwistedFate.DefPoint + 10;
                        break;

                    //3번 카드 맹세의 효과 발동 On, 방어력을 20 증가시키고 피해량을 1.5배 증가시킵니다.
                    case 3:
                        isDecisionActive = true;
                        TwistedFate.DefPoint = TwistedFate.DefPoint + 20;
                        pDamage *= 1.5;
                        break;

                    //5번 카드 버서크의 효과 발동 On, 피해량을 2.5배 증가시킵니다.
                    case 5:
                        isBerserkActive = true;
                        pDamage *= 2.5;
                        break;

                    //7번 카드 더블업의 효과 발동 On, 계산은 이 부분에서 진행하지 않습니다.
                    case 7:
                        isDoubleUpActive = true;
                        break;

                    //8번 카드 리버커의 효과 발동 On, 행동 포인트를 3 증가시킵니다.
                    case 8:
                        isRecoverActive = true;
                        TwistedFate.ActPoint = TwistedFate.ActPoint + 3;
                        break;

                    //9번 카드 리플렉트의 효과 발동 On, 상대의 행동이 공격일 경우 효과를 발동시킵니다.
                    //상대의 행동이 공격이 아닌 경우 피해를 입힐 수 없으며, 자신은 30의 데미지를 추가로 받습니다.
                    //사실 리플렉트라기보다는 패링에 가깝지만, 처음 생각난 단어는 리플렉트였습니다.
                    case 9:
                        isReflectActive = true;
                        if (enemies[phase].TurnAct == Act.공격 || enemies[phase].TurnAct == Act.연타 || enemies[phase].TurnAct == Act.강타)
                        if (enemies[phase].TurnAct != Act.공격)
                        {
                            pDamage = 0;
                            TwistedFate.DefPoint = TwistedFate.DefPoint - 30;
                        }
                        break;

                    //11번 카드 프리페어의 효과 발동 On, 계산은 여기에서 실행하지 않습니다.
                    case 11:
                        isPrepareActive = true;
                        break;

                    //12번 카드 셔플&힐의 경우, 전용 함수를 발동하여 조커를 포함한 덱 셔플을 진행합니다.
                    //또한, 체력과 행동포인트를 회복합니다.
                    case 12:
                        deck = QueenShuffle(6);
                        TwistedFate.HealthPoint = TwistedFate.HealthPoint + 500;
                        TwistedFate.ActPoint = TwistedFate.ActPoint + 6;
                        break;

                    //14번 카드 조커의 경우, 상대에게 입힌 피해량만큼 자신의 생명력을 회복합니다.
                    case 14:
                        Console.WriteLine("조커가 공격한 상대의 피를 빨아들여 당신의 생명력을 회복시킵니다");
                        TwistedFate.HealthPoint = TwistedFate.HealthPoint + pDamage;
                        break;
                }

                //플레이어가 선택한 카드의 파괴력과 적의 행동 위력 이상이면 공격 이외의 상대의 행동을 파훼합니다.
                if (power >= enemies[phase].Power)
                {
                    Console.WriteLine("적의 행동 위력 이상의 파괴력을 가진 카드를 사용하여 적의 행동에 영향을 가합니다.");

                    //적의 체력을 피해량만큼 감소시킵니다.
                    enemies[phase].HealthPoint = enemies[phase].HealthPoint - pDamage*dmgMultiplyer;
                    Thread.Sleep(1500);
                    Console.WriteLine($"적에게 {pDamage}만큼의 피해를 입혔습니다.");
                    Thread.Sleep(1500);
                }

                //플레이어가 선택한 카드의 파괴력이 적의 행동 위력보다 낮으면 상대의 행동에 따라 피해가 달라집니다.
                else
                {
                    Console.WriteLine("적보다 낮은 파괴력을 가진 카드를 사용하여 적의 행동에 영향을 주지 못하였습니다.");
                    if (enemies[phase].TurnAct == Act.방어)
                    {
                        Console.WriteLine($"적이 공격을 방어하여 피해를 {enemies[phase].DmgReduction * 100}%만큼 감소시켰습니다.");
                        Console.WriteLine($"적에게 {pDamage * enemies[phase].DmgReduction}만큼의 피해를 입혔습니다.");

                        //피해량에 적의 데미지 경감율을 곱하여 해당 값을 적의 체력에서 감소시킵니다.
                        enemies[phase].HealthPoint = enemies[phase].HealthPoint - pDamage * enemies[phase].DmgReduction;
                        Thread.Sleep(1500);
                    }
                    else if (enemies[phase].TurnAct == Act.회피)
                    {
                        Console.WriteLine("적이 공격을 회피하여 피해를 받지 않았습니다.");

                        //공격을 회피하여 피해를 입지 않습니다.
                        pDamage = 0;
                        Thread.Sleep(1500);
                    }
                    else if (enemies[phase].TurnAct == Act.폭주)
                    {
                        Console.WriteLine("적에게서 뿜어져 나오는 기운이 심상치 않습니다.");

                        //폭주의 트리거를 활성화합니다.
                        isBossBerserkActive = true;
                        //보스의 피해량이 2배 상승합니다.
                        enemies[phase].AtkPoint *= 2;
                        Thread.Sleep(1500);
                    }
                }

                //체력이 0에 도달했을 경우, 다음 페이즈를 불러옵니다.
                if (enemies[phase].HealthPoint <= 0)
                {
                    Console.Clear();
                    Thread.Sleep(1500);
                    Console.WriteLine("적을 쓰러트렸습니다. 다음 지역으로 이동합니다.");
                    Thread.Sleep(1500);
                    phase++;
                    pattern = 0;
                }

                //체력이 0에 도달하지 않았을 때 적의 행동이 공격이었다면, 적의 공격력 * 행동의 배율만큼 플레이어의 체력을 깎습니다.
                else if (enemies[phase].TurnAct == Act.공격 || enemies[phase].TurnAct == Act.연타 || enemies[phase].TurnAct == Act.강타)
                {
                    Console.Clear();
                    Thread.Sleep(1500);
                    Console.WriteLine("적이 당신을 공격합니다!");
                    //리플렉트 효과 적용 중인 경우, 적의 공격을 반사하는 데에 성공했다는 표기. 이후 공격 패턴에 영향을 끼칩니다.
                    if(isReflectActive == true)
                    {
                        Console.WriteLine("리플렉트의 효과를 사용하는 데 성공하여 적에게 피해를 고스란히 돌려줍니다.");
                    }
                    switch(enemies[phase].TurnAct)
                    {
                        case Act.공격:
                            if(isReflectActive == true)
                            {
                                Console.WriteLine("리플렉트의 효과로 공격을 반사하였습니다!");
                                enemies[phase].HealthPoint = enemies[phase].HealthPoint - enemies[phase].AtkPoint;
                            }
                            else
                            {
                                TwistedFate.HealthPoint = TwistedFate.HealthPoint - enemies[phase].AtkPoint;
                            }
                            break;
                        case Act.연타:
                            if(phase == 2)
                            {
                                if (isReflectActive == true)
                                {
                                    Console.WriteLine("리플렉트의 효과로 공격을 반사하였습니다!");
                                    enemies[phase].HealthPoint = enemies[phase].HealthPoint - (double)enemies[phase].AtkPoint* 1.6;
                                }
                                else
                                {
                                    TwistedFate.HealthPoint = TwistedFate.HealthPoint - (double)enemies[phase].AtkPoint * 1.6;
                                }
                            }
                            else if(phase == 3)
                            {
                                if (isReflectActive == true)
                                {
                                    Console.WriteLine("리플렉트의 효과로 공격을 반사하였습니다!");
                                    enemies[phase].HealthPoint = enemies[phase].HealthPoint - (double)enemies[phase].AtkPoint* 1.8;
                                }
                                else
                                {
                                    TwistedFate.HealthPoint = TwistedFate.HealthPoint - (double)enemies[phase].AtkPoint * 1.8;
                                }
                            }
                            break;
                        case Act.강타:
                            // 리플렉트 버프 지속 상태인 경우, 공격을 반사시켜 적에게 피해를 입힙니다.
                            if (isReflectActive == true)
                            {
                                Console.WriteLine("리플렉트의 효과로 공격을 반사하였습니다!");
                                enemies[phase].HealthPoint = enemies[phase].HealthPoint - (double)enemies[phase].AtkPoint* 1.5;
                            }

                            // 리플렉트 버프 지속 상태가 아니라면, 파괴력이 적 행동 위력 이상인 경우 틈을 노려 공격 준비 상태의 적을 무력화합니다.
                            else if (power >= enemies[phase].Power)
                            {
                                Console.WriteLine("상대의 행동 위력보다 더 큰 파괴력의 공격이 성공적으로 공격을 끊어냈습니다!");
                            }

                            //리플렉트 버프 지속 상태가 아니고, 파괴력이 적 행동 위력보다 낮다면 적이 준비를 끝마치고 공격에 성공합니다.
                            else
                            {
                                Console.WriteLine("적의 충전을 막지 못하였습니다.");
                                TwistedFate.HealthPoint = TwistedFate.HealthPoint - (double)enemies[phase].AtkPoint * 1.5;
                            }
                            break;
                    }

                    //리플렉트 효과 적용 중인 상태에서는 적의 체력을, 그 외의 경우 캐릭터의 체력을 보여줍니다.
                    if(isReflectActive == true)
                    {
                        Console.WriteLine($"현재 적의 체력은 {enemies[phase].HealthPoint}입니다.");
                    }
                    else
                    {
                        Console.WriteLine($"현재 당신의 체력은 {TwistedFate.HealthPoint}입니다.");
                    }
                    Thread.Sleep(1500);
                }

                //플레이어의 체력이 0이 되면 게임을 종료
                if(TwistedFate.HealthPoint <= 0)
                {
                    FailEnd();
                    isGameOn = false;
                }
                #endregion
                #region 적 행동 설정
                
                //적의 종류에 따라, 상대의 행동 패턴을 형성합니다

                //첫 번째 적의 행동 패턴을 2가지로 제한합니다.
                if (phase == 0)
                {
                    switch(pattern)
                    {
                        case 0:
                            pattern++;
                            enemies[phase].Power = Power.Next(0, 9);
                            enemies[phase].TurnAct = (Act)pattern;
                            break;
                        case 1:
                            pattern--;
                            enemies[phase].Power = Power.Next(0, 9);
                            enemies[phase].TurnAct = (Act)pattern;
                            break;
                    }
                }

                //두 번째 적의 행동 패턴을 3가지로 제한합니다.
                else if(phase == 1)
                {

                    switch (pattern)
                    {
                        case 0:
                        case 1:
                            pattern++;
                            enemies[phase].Power = Power.Next(2, 10);
                            enemies[phase].TurnAct = (Act)pattern;
                            break;
                        case 2:
                            pattern = 0;
                            enemies[phase].Power = Power.Next(2, 10);
                            enemies[phase].TurnAct = (Act)pattern;
                            break;
                    }
                }

                //세 번째 적의 행동 패턴을 4가지로 제한합니다.
                else if(phase == 2)
                {
                    switch (pattern)
                    {
                        case 0:
                        case 1:
                        case 2:
                            pattern++;
                            enemies[phase].Power = Power.Next(3, 11);
                            enemies[phase].TurnAct = (Act)pattern;
                            break;
                        case 3:
                            pattern = 0;
                            enemies[phase].Power = Power.Next(3, 11);
                            enemies[phase].TurnAct = (Act)pattern;
                            break;
                    }
                }

                //4번째 적은 모든 행동 패턴을 사용합니다.
                else if(phase == 3)
                {
                    switch (pattern)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            pattern++;
                            enemies[phase].Power = Power.Next(4, 14);
                            enemies[phase].TurnAct = (Act)pattern;
                            break;
                        case 5:
                            pattern = 0;
                            enemies[phase].Power = Power.Next(4, 14);
                            enemies[phase].TurnAct = (Act)pattern;
                            break;
                    }
                }
                #endregion
                #region 턴 넘김

                //만약 사용한 카드가 유일하게 효과가 있는 카드였다면, 카드를 새로 보충합니다.
                if(deck[0] == 0 && deck[1] == 0 && deck[2] == 0 && deck[3] == 0 && deck[4] == 0 && deck[5] == 0)
                {
                    Console.WriteLine("패를 전부 사용하여 재충전합니다.");
                    deck = Shuffle(6);
                    Thread.Sleep(1500);
                }

                //진행 완료 후 초기 화면 상태로 돌아가기 위해 현재 콘솔에 남아있는 것들을 전부 삭제합니다.
                Console.Clear();

                //게임의 엔딩 시점인 4번째 적 격파 상태 확인 시 엔딩을 출력하고 게임을 종료합니다.
                if(phase >= 4)
                {
                    RealEnd();
                    isGameOn = false;
                }
                #endregion
            }
            #endregion
            Thread.Sleep(1000); // 1초
        }
    }
}
