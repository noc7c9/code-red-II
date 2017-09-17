﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noc7c9.TheDigitalFrontier {

    public class AudioOnDestruction : MonoBehaviour {

        public AudioClip clip;

        void OnDestroy() {
            if (clip != null) {
                AudioSource.PlayClipAtPoint(clip, transform.position);
            }
        }

    }

}