using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRequest {

	private WWW m_WWW;
	private string m_URL;

	public CRequest (string url)
	{
		this.m_WWW = new WWW (url);
	}

	public void Get (Action<CResult> complete, Action<string> error, Action<float> process) {
#if TEST_ERROR
		if (error != null) {
			error("ERROR: TEST_ERROR...");
		}
#else
		CHandleEvent.Instance.AddEvent (this.HandleGet (complete, error, process), null);
#endif
	}

	public IEnumerator HandleGet(Action<CResult> complete, Action<string> error, Action<float> process) {
		if (Application.internetReachability != NetworkReachability.NotReachable) {
			while (this.m_WWW.isDone == false) {
				if (process != null) {
					process (this.m_WWW.progress);
					yield return WaitHelper.WaitFixedUpdate;
				}
			}
			yield return this.m_WWW;
			if (string.IsNullOrEmpty (this.m_WWW.error) == false) {
				if (error != null) {
					error (this.m_WWW.error);
				}
				this.m_WWW.Dispose ();
			} else {
				if (complete != null) {
					complete (new CResult (this.m_WWW));
				}
			}
		} else {
			if (error != null) {
				error ("Error: Connect error, please check connect internet.");
			}
			this.m_WWW.Dispose ();
		}
		yield return WaitHelper.WaitFixedUpdate;
	}

}
