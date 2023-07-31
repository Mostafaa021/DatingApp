import { Photo } from "./photo";

export interface Member {
    id: number;
    userName: string;
    photoUrl: string;
    age: number;
    created: string;
    lastActive: string;
    gender: string;
    introduction: string;
    lookingFor: string;
    knownAs: string;
    country: string;
    city: string;
    interests: string;
    photos: Photo[];
  }
  
