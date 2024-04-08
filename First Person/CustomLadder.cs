using UnityEngine;
using DG.Tweening;
    public class CustomLadder : ItemScript
    {
        public float climbingSpeed = 0.5f;
        public KeyCode climbUpKey = KeyCode.W;
        public KeyCode climbDownKey = KeyCode.S;
        private BoxCollider collider;
        private bool isClimbing = false;
        private Transform playerTransform;
        public Transform startPos;
        private int _climbedSteps = 0;

        void Start()
        {
            collider = GetComponent<BoxCollider>();
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform; 
        }

        void Update()
        {
            if (isClimbing)
            {
                if (Input.GetKey(climbUpKey))
                {
                    ClimbUp();
                }
                if (Input.GetKey(climbDownKey))
                {
                    ClimbDown();
                }
            }
        }

        public override void Interact()
        {
            base.Interact();
            In();
        }

        public override string DisplayMessage()
        {
            return MessageToShow;
        }
        private void In()
        {
            GameManager.Instance._canControll = false;
            collider.enabled = false;
            isClimbing = true;
            playerTransform.DOMove(startPos.position, 0.5f)
                .SetEase(Ease.InOutQuad)
                .OnComplete(() =>
                {
                    AudioManager.instance.PlayWoodFootStep(.5f);
                });
        }

        public void Out(Vector3 customPos)
        {
            if (!isClimbing)
                return;
            isClimbing = false;
            collider.enabled = true;

            playerTransform.DOMove(customPos, 0.5f)
                .SetEase(Ease.InOutQuad)
                .OnComplete(() =>
                {
                    GameManager.Instance._canControll = true;
                });
        }
        bool animationIsPlaying;

        void ClimbUp()
        {
            if (animationIsPlaying)
                return;
            _climbedSteps++;
            animationIsPlaying = true;
            playerTransform.DOMoveY(playerTransform.position.y + climbingSpeed, 0.5f)
                .SetEase(Ease.InOutQuad)
                .OnComplete(() =>
                {
                    animationIsPlaying = false;
                    AudioManager.instance.PlayWoodFootStep(.5f);
                });
        }

        void ClimbDown()
        {
            if(_climbedSteps == 0)
            {
                Out(startPos.position);
                return;
            }
            _climbedSteps--;
            if (animationIsPlaying)
                return;
            animationIsPlaying = true;
            playerTransform.DOMoveY(playerTransform.position.y - climbingSpeed, 0.5f)
                .SetEase(Ease.InOutQuad)
                .OnComplete(() =>
                {
                    animationIsPlaying = false;
                    AudioManager.instance.PlayWoodFootStep(.5f);
                });
        }
    }
