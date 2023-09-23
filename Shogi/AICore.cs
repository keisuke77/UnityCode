using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SocialPlatforms.Impl;

namespace Shogi
{
    public static class AICore
    {
        public static List<KomaInfo> ListCopy(this List<KomaInfo> originalList)
        {
            return originalList.Select(item => item.Clone()).ToList();
        }

        public static List<KomaInfo> GetVirtualMoveBoard(
            this List<KomaInfo> KomaInfos,
          Behavior behavior,
            Team CurrentTeam
        )
        {
            List<KomaInfo> VirtualBoard = KomaInfos.ListCopy();
            KomaInfo vPlaceKoma = VirtualBoard.GetKomaFromAdress(behavior.PlaceKoma.Adress);
            VirtualBoard.Add(new KomaInfo(MainSystem._AirKoma, vPlaceKoma.Adress, CurrentTeam));
            vPlaceKoma.Adress = behavior.PlacedKoma.Adress;
            VirtualBoard.Remove(VirtualBoard.GetKomaFromAdress(behavior.PlacedKoma.Adress));
            return VirtualBoard;
        }

        public static List<KomaInfo> GetVirtualMoveBoard(
            this List<KomaInfo> KomaInfos,
            Koma PlaceKoma,
            KomaInfo PlacedKoma,
            Team CurrentTeam
        )
        {
            Behavior behavior= new Behavior(  new KomaInfo(PlaceKoma, PlacedKoma.Adress, CurrentTeam),
                PlacedKoma);
            return KomaInfos.GetVirtualMoveBoard(
              behavior,
                CurrentTeam
            );
        }

        public static KeyValuePair<int, KeyValuePair<Koma, KomaInfo>> MostStackSelectBehavior(
            this List<KomaInfo> KomaInfos,
            int BoardSize,
            Team CurrentTeam,
            List<Koma> Stacks
        )
        {
            int bestScore = int.MinValue;
            KeyValuePair<Koma, KomaInfo> bestMove = default;

            foreach (var item in Stacks)
            {
                foreach (var RandomAir in KomaInfos.Where(koma => koma.Team == Team.None))
                {
                    KomaInfo PlaceKoma = new KomaInfo(item, RandomAir.Adress, CurrentTeam);
                    List<KomaInfo> firestBoard = KomaInfos.GetVirtualMoveBoard(item, RandomAir, CurrentTeam);

                    int myScore = PlaceKoma.GetPlaceableMostHighScore(BoardSize, CurrentTeam, firestBoard).Koma.Score;
                    int nextScore = firestBoard.GetMostBehavior(BoardSize, CurrentTeam.ReverseTeam(), false).Key;

                    int result = myScore - nextScore;
                    if (result > bestScore)
                    {
                        bestScore = result;
                        bestMove = new KeyValuePair<Koma, KomaInfo>(item, RandomAir);
                    }
                }
            }

            return new KeyValuePair<int, KeyValuePair<Koma, KomaInfo>>(bestScore, bestMove);
        }

        //ゲーム理論のミニマックス法
        public static KeyValuePair<int, Behavior> GetMiniMaxBehavior(
            this List<KomaInfo> KomaInfos,
            int BoardSize,
            Team CurrentTeam,
            int Strength = 0
        )
        {
            int bestScore = int.MinValue;
            Behavior bestMove = default;

            foreach (var behavior in KomaInfos.GetAllWaysBehavior(BoardSize, CurrentTeam))
            {
                int currentScore = CalculateScoreForBehavior(KomaInfos, behavior, BoardSize, CurrentTeam, Strength);

                if (currentScore > bestScore)
                {
                    bestScore = currentScore;
                    bestMove = behavior.Value;
                }
            }

            Debug.Log(bestMove+""+bestScore);
            return new KeyValuePair<int, Behavior>(bestMove.PlacedKoma.Koma.Score, bestMove);
        }
     private static List<List<Behavior>> GetAllWaysBehaviorFuture( this List<KomaInfo> KomaInfos,
            int BoardSize,
            Team CurrentTeam,
            int Strength = 0)
            {
                List<List<Behavior>> result=new List<List<Behavior>>();

  foreach (var behavior in KomaInfos.GetAllWaysBehavior(BoardSize, CurrentTeam))
            {
List<Behavior> behaviors=new List<Behavior>();
behaviors.Add(behavior.Value);
var nextBoard= KomaInfos.GetVirtualMoveBoard(behavior.Value,CurrentTeam);
CurrentTeam=CurrentTeam.ReverseTeam();
  foreach (var item in nextBoard.GetAllWaysBehavior(BoardSize, CurrentTeam))
  {
behaviors.Add(item.Value);
  }


     }

     return null;
            }

public static  List<List<Behavior>> allBehavior;
            public static List<Behavior> LoopBehavior( this List<KomaInfo> KomaInfos,
            int BoardSize,  Team CurrentTeam,List<Behavior> behaviors,
            int Strength = 0)
            {
                if (Strength==0)
{
    return behaviors;
}

  foreach (var behavior in KomaInfos.GetAllWaysBehavior(BoardSize, CurrentTeam))
  {
behaviors.Add(behavior.Value);
Strength--;
var nextBoard= KomaInfos.GetVirtualMoveBoard(behavior.Value,CurrentTeam);
CurrentTeam=CurrentTeam.ReverseTeam();



  }
  return null;

            }
      

   




     private static List<KomaInfo> VirtualBoardChange(  List<KomaInfo> KomaInfos,
            int BoardSize,
            Team CurrentTeam,List<Behavior> behaviors)
     {
       List<KomaInfo> CurrentVBoard=new List<KomaInfo>();
foreach (var item in behaviors)
{
    CurrentVBoard=KomaInfos.GetVirtualMoveBoard(item,CurrentTeam);
    CurrentTeam=CurrentTeam.ReverseTeam();
}
return CurrentVBoard;
     }


        private static int CalculateScoreForBoard(
            List<KomaInfo> KomaInfos,
            int BoardSize,
            Team CurrentTeam
        )
        {
            int Score=0;
               foreach (var behavior in KomaInfos.GetAllWaysBehavior(BoardSize, CurrentTeam))
            {
               Score+= behavior.Key;
            }
foreach (var item in KomaInfos.Where(x=>x.Team==CurrentTeam))
{
    Score+=item.Koma.Score;
}
          

return Score;
        }

        private static int CalculateScoreForBehavior(
            List<KomaInfo> KomaInfos,
            KeyValuePair<int, Behavior> behavior,
            int BoardSize,
            Team CurrentTeam,
            int Strength
        )
        {
            Team FirstTeam= CurrentTeam;
            int score = 0;
            List<KomaInfo> VirtualBoard = KomaInfos.ListCopy();
            VirtualBoard = VirtualBoard.GetVirtualMoveBoard(behavior.Value, CurrentTeam);
            var KomaPlaceAndPlaced = behavior.Value;
            score += behavior.Key;

            for (int i = 0; i <= Strength; i++)
            { 
                CurrentTeam = CurrentTeam.ReverseTeam();
                var nextMove = VirtualBoard.GetMostBehavior(BoardSize, CurrentTeam, false);
                score -= nextMove.Key*(FirstTeam==CurrentTeam?1:-1);
                VirtualBoard = VirtualBoard.GetVirtualMoveBoard(nextMove.Value, CurrentTeam);
            }

            return score;
        }

        public static KeyValuePair<int, Behavior> GetSeeFutureBehavior(
            this List<KomaInfo> KomaInfos,
            int BoardSize,
            Team CurrentTeam
        )
        {
            int bestScore = int.MinValue;
            Behavior bestMove = default;

            foreach (var behavior in KomaInfos.GetAllWaysBehavior(BoardSize, CurrentTeam))
            {
                var KomaPlaceAndPlaced = behavior.Value;
                int currentScore = behavior.Key + KomaInfos.GetVirtualMoveBoard(KomaPlaceAndPlaced, CurrentTeam).GetMostBehavior(BoardSize, CurrentTeam, false).Key;

                if (currentScore > bestScore)
                {
                    bestScore = currentScore;
                    bestMove = behavior.Value;
                }
            }

            Debug.Log(bestMove);
            return new KeyValuePair<int, Behavior>(bestScore, bestMove);
        }
    }
}
