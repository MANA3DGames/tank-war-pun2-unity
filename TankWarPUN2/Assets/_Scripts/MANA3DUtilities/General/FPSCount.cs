using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MANA3DGames
{
	public class FPSRecord
	{
		public int minFPS;
		public int maxFPS;
		public List<int> fpsAvg;
		public bool fpsSuddenDrop;

		public FPSRecord()
		{
			minFPS = 60;
			maxFPS = 0;
			fpsSuddenDrop = false;
			fpsAvg = new List<int>( 10 );
		}
	}

	public class FPSCount
	{
		private const float fpsMeasurePeriod = 0.5f;
		private int m_FpsAccumulator = 0;
		private float m_FpsNextPeriod = 0;
		private int m_CurrentFps;
		private const string display = "{0} <size=70%>FPS</size>";
		private TextMeshProUGUI m_Text;

		FPSRecord[,] gFPSRecord;


		public FPSCount( TextMeshProUGUI textGUI )
		{
			m_FpsNextPeriod = Time.realtimeSinceStartup + fpsMeasurePeriod;
			m_Text = textGUI;

			gFPSRecord = new FPSRecord[6,10];
			for ( int i = 0; i < 6; i++ ) 
			{
				for ( int j = 0; j < 10; j++ ) 
					gFPSRecord[i,j] = new FPSRecord();
			}
		}

		public void Update()
		{
			// measure average frames per second
			m_FpsAccumulator++;

			if ( Time.realtimeSinceStartup > m_FpsNextPeriod )
			{
				m_CurrentFps = (int)( m_FpsAccumulator/fpsMeasurePeriod );
				m_FpsAccumulator = 0;
				m_FpsNextPeriod += fpsMeasurePeriod;
				m_Text.text = string.Format( display, m_CurrentFps );
			}
		}


		void RecoredFPS( int shipGen, int level )
		{
			var record = gFPSRecord[shipGen,level];
			if ( record.minFPS > m_CurrentFps )
			{
				if ( record.fpsSuddenDrop )
				{
					record.minFPS = m_CurrentFps;
					record.fpsSuddenDrop = false;
				}
				else
				{
					if ( record.minFPS - 10 > 0 && record.minFPS - 10 > m_CurrentFps || m_CurrentFps == 0 )	
						record.fpsSuddenDrop = true;
					else
						record.minFPS = m_CurrentFps;
				}
			}

			if ( record.maxFPS < m_CurrentFps )
				record.maxFPS = m_CurrentFps;


			if ( record.fpsAvg.Count >= 10 )
			{
				record.fpsAvg.RemoveAt( 9 );
			}

			// If there is no sudden drop in FPS.
			if ( !record.fpsSuddenDrop )
				record.fpsAvg.Add( m_CurrentFps );
		}

		public int GetMinFPS( int shipGen, int level )
		{
			return gFPSRecord[shipGen,level].minFPS;
		}
		public int GetMaxFPS( int shipGen, int level )
		{
			return gFPSRecord[shipGen,level].maxFPS;
		}

		public int GetAverage( int shipGen, int level )
		{
			var record = gFPSRecord[shipGen,level];
			if ( record.fpsAvg.Count == 0 )
				return ( record.maxFPS + record.minFPS ) / 2;
			
			int sum = 0;
			for ( int i = 0; i < record.fpsAvg.Count; i++ )
				sum += record.fpsAvg[i];

			return sum / record.fpsAvg.Count;
		}
	}
}