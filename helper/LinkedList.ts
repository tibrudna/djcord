class ListNode<T> {
    private _content: T;
    private _next: ListNode<T>;

    constructor(content: T) {
        this._content = content;
        this._next = null;
    }

    getContent(): T {
        return this._content;
    }

    getNext(): ListNode<T> {
        return this._next;
    }

    setNext(node: ListNode<T>): void {
        this._next = node;
    }
}

class LinkedList<T> {
    private _head: ListNode<T>;
    private _tail: ListNode<T>;
    private _length: number;

    constructor() {
        this._head = null;
        this._tail = null;
        this._length = 0;
    }

    push(content: T): void {
        let node = new ListNode(content);

        if (this._length == 0) {
            this._head = node;
            this._tail = node;
        }
        else {
            let oldNode = this._tail;
            oldNode.setNext(node);
            this._tail = node;
        }

        this._length++;
    }

    pop(): T {
        let node = this._head;
        this._head = node.getNext();

        this._length--;
        if (this._length == 0) {
            this._tail = null;
        }

        return node.getContent();
    }

    length(): number {
        return this._length;
    }
}

export { LinkedList }