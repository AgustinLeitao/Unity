using UnityEngine;

public class BarcoFisico : MonoBehaviour {

	private bool hundido;
	private bool animTermino;
	private Quaternion newRot;
	private Vector3 newPos;

	// Use this for initialization
	void Start () {
		hundido = false;
		animTermino = false;
		newPos = new Vector3 (this.transform.position.x, this.transform.position.y - 30f, this.transform.position.z);
		newRot = Quaternion.Euler(new Vector3 (this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z - 30f));
	}
	
	// Update is called once per frame
	void Update () {
		if (hundido && animTermino && Vector3.Distance (this.transform.position, newPos) > 2f) {
			this.transform.rotation = Quaternion.Slerp (this.transform.rotation, newRot, Time.deltaTime * 0.5f);
			this.transform.position = this.transform.position - (new Vector3 (0f, 0.05f, 0f));
		}
	}

	public bool Hundido {
		get {
			return this.hundido;
		}
		set {
			hundido = value;
		}
	}

	public bool AnimTermino {
		get {
			return this.animTermino;
		}
		set {
			animTermino = value;
		}
	}
}
