
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UGUI = UnityEngine.UI;

public class PhysicsTest2 : MonoBehaviour
{
    [SerializeField] private GameObject ball;
    [SerializeField] private GameObject field;
    [SerializeField] private LineRenderer line;
    [SerializeField] private float simulateFrame;
    [SerializeField] private float simulateSpeed;
    [SerializeField] private float ballPotential;
    [SerializeField] private StickController stick;
    [SerializeField] private UGUI.Button shotBtn;
    [SerializeField] private UGUI.Button restartBtn;

    private Scene _mainScene;
    private PhysicsScene _mainPhysicsScene;

    private Scene _subScene;
    private PhysicsScene _subPhysicsScene;

    private Vector3 _ballPosition;

    private BallActor _ballActor;

    private GameObject _hiddenBall;
    private BallActor _hiddenBallActor;

    private float _timer;

    private void Awake()
    {
        Physics.autoSimulation = false;

        _mainScene = SceneManager.GetActiveScene();
        _mainPhysicsScene = _mainScene.GetPhysicsScene();

        _subScene = SceneManager.CreateScene("PhysicsScene1", new CreateSceneParameters(LocalPhysicsMode.Physics3D));
        _subPhysicsScene = _subScene.GetPhysicsScene();

        _ballActor = ball.GetComponent<BallActor>();
        _ballPosition = new Vector3(0, 2, 0);

        stick.onDrag += OnMove;
        shotBtn.onClick.AddListener(OnShot);
        restartBtn.onClick.AddListener(OnRestart);
    }

    private void OnRestart()
    {
        _ballPosition = new Vector3(0, 2, 0);

        _ballActor.Stop();
        _ballActor.Warp(_ballPosition);
        CreateTrajectory();
    }

    private void OnShot()
    {
        _ballActor.Play(ballPotential);
    }

    private void OnMove(Vector3 dir)
    {
        var d = dir.normalized;
        var f = new Vector3(d.x, 0, d.y) * Time.deltaTime;

        _ballPosition += f;
        _ballActor.Warp(_ballPosition);
        CreateTrajectory();
    }

    private void Start()
    {
        CreateTrajectory();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _ballActor.Play(ballPotential);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            _ballPosition = new Vector3(0, 2, 0);

            _ballActor.Stop();
            _ballActor.Warp(_ballPosition);
            CreateTrajectory();
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _ballPosition += new Vector3(-0.1f, 0, 0);
            _ballActor.Warp(_ballPosition);
            CreateTrajectory();
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            _ballPosition += new Vector3(0.1f, 0, 0);
            _ballActor.Warp(_ballPosition);
            CreateTrajectory();
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            _ballPosition += new Vector3(0, 0, 0.1f);
            _ballActor.Warp(_ballPosition);
            CreateTrajectory();
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            _ballPosition += new Vector3(0, 0, -0.1f);
            _ballActor.Warp(_ballPosition);
            CreateTrajectory();
        }

        _timer += Time.deltaTime * 2f;
        while (_timer >= Time.fixedDeltaTime)
        {
            _timer -= Time.fixedDeltaTime;
            _mainPhysicsScene.Simulate(Time.fixedDeltaTime);
            _ballActor.Simulate(Time.fixedDeltaTime);
        }
    }

    private void CreateTrajectory()
    {
        if (_hiddenBall == null)
        {
            _hiddenBall = Instantiate(ball);
            _hiddenBall.name = "HiddenBall";

            _hiddenBallActor = _hiddenBall.GetComponent<BallActor>();
            _hiddenBallActor.SetRenderable(false);

            SceneManager.MoveGameObjectToScene(_hiddenBall, _subScene);

            var hiddenField = Instantiate(field);
            SceneManager.MoveGameObjectToScene(hiddenField, _subScene);

            var ren2 = hiddenField.GetComponentsInChildren<Renderer>();
            foreach (var r in ren2)
                r.enabled = false;
        }

        if (_hiddenBall != null)
        {
            _hiddenBallActor.Warp(_ballActor.position);
            _hiddenBallActor.Play(ballPotential);

            List<Vector3> points = new List<Vector3>();
            for (int i = 0; i < simulateFrame; i++)
            {
                float dt = Time.fixedDeltaTime;
                _subPhysicsScene.Simulate(dt);
                _hiddenBallActor.Simulate(dt);

                if (points.Count == 0 || (0.2f < (points[points.Count - 1] - _hiddenBallActor.position).magnitude))
                    points.Add(_hiddenBallActor.position);
            }

            line.positionCount = points.Count;
            line.SetPositions(points.ToArray());
        }
    }
}
