using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class FunctionalInterpreter : MonoBehaviour{

	public float clock 					 = 1.0f;
	public int 	 initialInstructionCount = 100;
	public float strength = 5.0f;
	public bool impulseBased=false;

	// <Behaviour>

	/* randomly initialize the program */

	void Start(){

		List<Instruction> program=new List<Instruction>();
		for(int i=0;i<initialInstructionCount;i++){
			program.Add ( CreateRandomInstruction() );
		}

		Debug.Log ("Start program");
		StartCoroutine("Run",program);

	}

	public Instruction CreateRandomInstruction(){

		int N = Random.Range (0,5);
		switch(N){
		case 0: return new Instruction ("left");
		case 1: return new Instruction ("right");
		case 2: return new Instruction ("forward");
		case 3: return new Instruction ("backward");
		case 4: return new Instruction ("up");
		default: return null;
		}

	}

	public IEnumerator Run(List<Instruction> program){

		foreach(var instruction in program){
			Debug.Log ("run instruction...");
			Run (instruction);
			yield return new WaitForSeconds(clock);
		}

	}

	public object Run(Instruction i){

		Debug.Log ("Exec "+i.name);

		if(i.name=="left") 				AddImpulse( -1.0f, 0.0f,  0.0f );
		else if(i.name=="right")		AddImpulse(  1.0f, 0.0f,  0.0f );
		else if(i.name=="forward")		AddImpulse(  0.0f, 0.0f,  1.0f );
		else if(i.name=="backward")		AddImpulse(  0.0f, 0.0f, -1.0f );
		else if(i.name=="up") 			AddImpulse(  0.0f, 1.0f,  0.0f );
		else if(i.name=="sum"){
		
			float s = 0.0f;
			foreach(var e in i.arguments){
				s+=(float)e;
			} 
			return s;
		
		}else if(i.name=="product"){

			float s = 1.0f;
			foreach(var e in i.arguments){
				s+=(float)e;
			}
			return s;

		}else{

			Debug.LogError ("illegal instruction: "+i.name);
			return null;

		}

		return null;

	}

	private void AddImpulse(float x, float y, float z){

		Vector3 F = new Vector3( x, y, z)*strength;
		Debug.Log ("add impulse:"+F);
		if(impulseBased){
			GetComponent<Rigidbody>().AddForce(F,ForceMode.Impulse);
		}else{
			GetComponent<Rigidbody>().velocity = F;
		}

	}

}

public class Instruction{

	public string name;
	public List<object> arguments;

	public Instruction(string name){

		this.name=name;

	}

	public Instruction(string name, params object[] args){

		this.name=name;
		arguments= new List<object>(args);

	}

}