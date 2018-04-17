import { Tag } from "../tags";

export class Note { 
  public noteId:any;
  public title: string;
  public body: string;
  public tags?: Array<Tag> = [];
}
