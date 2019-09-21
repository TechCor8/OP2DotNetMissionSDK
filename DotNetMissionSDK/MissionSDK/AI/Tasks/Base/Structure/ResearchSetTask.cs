using DotNetMissionSDK.State.Snapshot;
using System.Collections.Generic;

namespace DotNetMissionSDK.AI.Tasks.Base.Structure
{
	/// <summary>
	/// This task completes a set of research topics including all prerequisite research topics.
	/// </summary>
	public class ResearchSetTask : Task
	{
		public class ResearchTopic
		{
			public int topic							{ get; private set; }
			public bool shouldResearchPrerequisites		{ get; private set; }

			public ResearchTopic(int topic, bool shouldResearchPrerequisites)
			{
				this.topic = topic;
				this.shouldResearchPrerequisites = shouldResearchPrerequisites;
			}
		}

		private List<ResearchTopic> m_TopicsToResearch = new List<ResearchTopic>();

		private ResearchTask m_ResearchTask;

		/// <summary>
		/// Sets the topics to research.
		/// </summary>
		/// <param name="researchTopics">The techIndex of the technology to research.</param>
		public void SetTopicsToResearch(int[] researchTopics)
		{
			m_TopicsToResearch = new List<ResearchTopic>(researchTopics.Length);

			for (int i=0; i < researchTopics.Length; ++i)
				m_TopicsToResearch.Add(new ResearchTopic(researchTopics[i], true));
		}

		public void SetTopicsToResearch(ResearchTopic[] researchTopics)
		{
			m_TopicsToResearch = new List<ResearchTopic>(researchTopics);
			m_TopicsToResearch.AddRange(researchTopics);
		}

		
		public ResearchSetTask(int ownerID, int[] researchTopics) : base(ownerID)			{ SetTopicsToResearch(researchTopics); }
		public ResearchSetTask(int ownerID, ResearchTopic[] researchTopics) : base(ownerID) { SetTopicsToResearch(researchTopics); }

		public override bool IsTaskComplete(StateSnapshot stateSnapshot)
		{
			PlayerState owner = stateSnapshot.players[ownerID];

			for (int i=0; i < m_TopicsToResearch.Count; ++i)
			{
				m_ResearchTask.topicToResearch = m_TopicsToResearch[i].topic;
				m_ResearchTask.shouldResearchPrerequisites = m_TopicsToResearch[i].shouldResearchPrerequisites;

				// If this topic is complete or can't be researched, try next topic
				if (!owner.CanColonyResearchTechnology(m_ResearchTask.topicToResearch) || m_ResearchTask.IsTaskComplete(stateSnapshot))
				{
					m_TopicsToResearch.RemoveAt(i--);
					continue;
				}

				// If topic has unresearched prerequisites, and we shouldn't research them, try next topic
				if (!m_TopicsToResearch[i].shouldResearchPrerequisites && IsMissingPrerequisite(stateSnapshot, owner, m_TopicsToResearch[i].topic))
					continue;

				// Research this topic
				return false;
			}

			return true;
		}

		public override void GeneratePrerequisites()
		{
			AddPrerequisite(m_ResearchTask = new ResearchTask(ownerID, -1));
		}

		protected override TaskResult PerformTask(StateSnapshot stateSnapshot, TaskRequirements restrictedRequirements, BotCommands unitActions)
		{
			return m_ResearchTask.PerformTaskTree(stateSnapshot, restrictedRequirements, unitActions);
		}

		private bool IsMissingPrerequisite(StateSnapshot stateSnapshot, PlayerState owner, int topic)
		{
			int[] requiredTopics = stateSnapshot.techInfo[topic].requiredTopics;

			for (int i=0; i < requiredTopics.Length; ++i)
			{
				if (!owner.HasTechnologyByIndex(requiredTopics[i]))
					return true;
			}

			return false;
		}
	}
}
