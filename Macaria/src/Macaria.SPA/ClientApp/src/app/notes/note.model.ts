import { Tag } from "../tags/tag.model";

export class Note { 
  public noteId:any;
  public title: string;
  public body: string;
  public tags?: Array<Tag> = [];
}
