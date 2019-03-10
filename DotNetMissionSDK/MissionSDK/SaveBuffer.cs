using System;
using System.Runtime.InteropServices;

namespace DotNetMissionSDK
{
	/// <summary>
	/// A wrapper for a save buffer in unmanaged code.
	/// </summary>
	/// <typeparam name="T">The data type to save. Must not contain reference types and should be packed.</typeparam>
	public class SaveBuffer<T> where T : new()
	{
		private IntPtr m_Handle;    // Handle to the unmanaged buffer

		public bool isLoaded				{ get; private set;							}

		public T saveData					{ get; private set;							}


		public IntPtr GetSaveBuffer()		{ return m_Handle;							}
		public int GetSaveBufferLength()	{ return Marshal.SizeOf<T>();				}

		/// <summary>
		/// Instantiates SaveBuffer and creates an unmanaged buffer handle.
		/// </summary>
		public SaveBuffer()
		{
			saveData = new T();

			// Create the unmanaged buffer
			int typeSize = Marshal.SizeOf<T>();

			m_Handle = Marshal.AllocHGlobal(typeSize);

			// Zero buffer
			for (int i=0; i < typeSize; ++i)
				Marshal.WriteByte(m_Handle, i, 0);
		}

		/// <summary>
		/// Loads the "saveData" member from the unmanaged buffer.
		/// </summary>
		public void Load()
		{
			Marshal.PtrToStructure(m_Handle, saveData);

			isLoaded = true;
		}

		/// <summary>
		/// Saves the "saveData" member to the unmanaged buffer.
		/// </summary>
		public void Save()
		{
			Marshal.StructureToPtr(saveData, m_Handle, true);
		}


		// --- Release ---
		public void Dispose()
		{
			Dispose(true);

			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if(disposing)
			{
				// Release managed objects
			}

			// Release unmanaged resources
			Marshal.FreeHGlobal(m_Handle);
		}

		~SaveBuffer()
		{
			Dispose(false);
		}
	}
}
