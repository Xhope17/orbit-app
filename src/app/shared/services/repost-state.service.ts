import { Injectable, signal } from '@angular/core';

export interface RepostEntry {
  repostId: string;
}

const STORAGE_KEY = 'orbit_reposts';

@Injectable({ providedIn: 'root' })
export class RepostStateService {
  private repostMap = signal<Map<string, RepostEntry>>(new Map());

  constructor() {
    this.loadFromStorage();
  }

  private loadFromStorage() {
    try {
      const raw = localStorage.getItem(STORAGE_KEY);
      if (raw) {
        const entries: [string, RepostEntry][] = JSON.parse(raw);
        this.repostMap.set(new Map(entries));
      }
    } catch {}
  }

  private saveToStorage() {
    try {
      localStorage.setItem(STORAGE_KEY, JSON.stringify(Array.from(this.repostMap().entries())));
    } catch {}
  }

  isReposted(originalPostId: string): boolean {
    return this.repostMap().has(originalPostId);
  }

  getRepostId(originalPostId: string): string | undefined {
    return this.repostMap().get(originalPostId)?.repostId;
  }

  markReposted(originalPostId: string, repostId: string) {
    this.repostMap.update((map) => {
      map.set(originalPostId, { repostId });
      return new Map(map);
    });
    this.saveToStorage();
  }

  removeRepost(originalPostId: string) {
    this.repostMap.update((map) => {
      map.delete(originalPostId);
      return new Map(map);
    });
    this.saveToStorage();
  }
}
