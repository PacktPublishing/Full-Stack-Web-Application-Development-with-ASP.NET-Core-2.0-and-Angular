import { Note } from "../notes/note.model";

export class Tag {
  public tagId: any;

  public name: string;

  public slug: string;

  public notes?: Array<Note> = [];
}
