using System;

namespace Assets.Scripts.Framework
{
	public class GameStateCtrl : Singleton<GameStateCtrl>
	{
		private StateMachine gameState = new StateMachine();

		public bool isBattleState
		{
			get
			{
				return this.gameState.TopState() is BattleState;
			}
		}

		public bool isLoadingState
		{
			get
			{
				return this.gameState.TopState() is LoadingState;
			}
		}

		public bool isLobbyState
		{
			get
			{
				return this.gameState.TopState() is LobbyState;
			}
		}

		public bool isHeroChooseState
		{
			get
			{
				return this.gameState.TopState() is HeroChooseState;
			}
		}

		public bool isLoginState
		{
			get
			{
				return this.gameState.TopState() is LoginState;
			}
		}

		public string currentStateName
		{
			get
			{
				IState currentState = this.GetCurrentState();
				return (currentState == null) ? "unkown state" : currentState.get_name();
			}
		}

		public void Initialize()
		{
			this.gameState.RegisterStateByAttributes<GameStateAttribute>(typeof(GameStateAttribute).get_Assembly());
		}

		public void Uninitialize()
		{
			this.gameState.Clear();
			this.gameState = null;
		}

		public void GotoState(string name)
		{
			string text = string.Format("GameStateCtrl Goto State {0}", name);
			DebugHelper.CustomLog(text);
			this.gameState.ChangeState(name);
		}

		public IState GetCurrentState()
		{
			return this.gameState.TopState();
		}
	}
}
