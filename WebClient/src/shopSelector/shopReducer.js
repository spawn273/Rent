import { CHANGE_SHOP } from './actions';

export default (previousState = 0, { type, payload }) => {
    if (type === CHANGE_SHOP) {
        return payload;
    }
    return previousState;
}
